using CsvHelper;
using DoCert.Contracts;
using DoCert.DataLayer;
using DoCert.DataLayer.Repositories;
using DoCert.Model;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Data.Patterns.UnitOfWorks;

namespace DoCert.Services;

public class DataService(IDonateRepository donateRepository,
    IDonorRepository donorRepository,
    IBankAccountRepository bankAccountRepository,
    IImportProfileRepository importProfileRepository,
    IUnitOfWork unitOfWork) : IDataService
{
    
    public async Task<DataFragmentResult<Donate>> GetDonatesDataFragmentAsync(DonateFilter filter, GridDataProviderRequest<Donate> request,
        CancellationToken cancellationToken = default)
    {
        var data = await donateRepository.GetFragmentAsync(filter, request, cancellationToken);
        return data.ToDataFragmentResult();
    }

    public async Task InsertDonatesAsync(List<Donate> donates, CancellationToken cancellationToken = default)
    {
        foreach (var donate in donates)
            unitOfWork.AddForInsert(donate);

        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task DeleteDonateAsync(Donate donate, CancellationToken cancellationToken = default)
    {
        unitOfWork.AddForDelete(donate);
        await unitOfWork.CommitAsync(cancellationToken);
    }
    
    public async Task<List<ImportProfile>> GetImportProfilesAsync(CancellationToken cancellationToken = default)
    {
        return await importProfileRepository.GetAllAsync(cancellationToken);
    }

    public async Task ImportDonatesFromCsvAsync(StreamReader streamReader, ImportProfile profile, CancellationToken cancellationToken = default)
    {
        var config = profile.GetCsvConfiguration();

        using var csv = new CsvReader(streamReader, config);

        var allDonors = await donorRepository.GetAllAsync(cancellationToken);
        var allAccounts = await bankAccountRepository.GetAllAsync(cancellationToken);
        
        if(config.HasHeaderRecord)
            await csv.ReadAsync(); //skip header;
        
        while (await csv.ReadAsync())
        {
            var line = csv.Parser.Record;
            
            if(line == null)
                continue;
            
            var amount = line.GetDecimal(profile.AmountColumnIndex, config.CultureInfo);
            var donorName = line.GetString(profile.DonorNameColumnIndex);
            var accNumber = line.GetString(profile.AccountNumberColumnIndex);
            
            if(amount <= 0)
                continue;
            
            if(string.IsNullOrWhiteSpace(donorName))
                continue;
            
            if(string.IsNullOrWhiteSpace(accNumber))
                continue;
            
            var donor = allDonors.FirstOrDefault(d => d.Name == donorName);// await donorRepository.GetDonorByNameAsync(donorName, cancellationToken);
            if(donor == null)
            {
                donor = new Donor
                {
                    Name = donorName,
                    Email = string.Empty
                };
                allDonors.Add(donor);
                unitOfWork.AddForInsert(donor);
            }
            
            
            var acc = allAccounts.FirstOrDefault(a => a.AccountNumber == accNumber); //await bankAccountRepository.GetAccountByNumberAsync(accNumber, cancellationToken);

            if (acc == null)
            {
                acc = new BankAccount
                {
                    AccountNumber = accNumber
                };
                allAccounts.Add(acc);
                unitOfWork.AddForInsert(acc);
            }

            var donate = new Donate
            {
                Donor = donor,
                Amount = line.GetDouble(profile.AmountColumnIndex, config.CultureInfo),
                Date = line.GetDateTime(profile.DateColumnIndex, config.CultureInfo),
                BankAccount = acc,
                VariableSymbol = line.GetString(profile.VariableSymbolColumnIndex),
                SpecificSymbol = line.GetString(profile.SpecificSymbolColumnIndex),
                ConstantSymbol = line.GetString(profile.ConstantSymbolColumnIndex),
                Message = line.GetString(profile.MessageColumnIndex)
            };
            
            unitOfWork.AddForInsert(donate);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);

    }
    
    
}