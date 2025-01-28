using DoCert.Contracts;
using DoCert.DataLayer;
using DoCert.DataLayer.Repositories;
using DoCert.Model;
using ExcelDataReader;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Data.Patterns.UnitOfWorks;

namespace DoCert.Services;

public class DataService(IDonateRepository donateRepository,
    IDonorRepository donorRepository,
    IAgendaRepository agendaRepository,
    IMailAccountRepository mailAccountRepository,
    ICertificateRepository certificateRepository,
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

    public async Task DeleteDonatesAsync(IEnumerable<Donate> donates, CancellationToken cancellationToken = default)
    {
        unitOfWork.AddRangeForDelete(donates);
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task ImportDonatesFromExcelAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        
        var allDonors = await donorRepository.GetAllAsync(cancellationToken);
        
        using var reader = ExcelReaderFactory.CreateReader(stream);

        //sheet 0
        reader.Read();
        while (reader.Read())
        {
            var donorName = reader.GetString(0);
            var date = reader.GetDateTime(1);
            var iban = reader.GetValue(2)?.ToString();
            var vs = reader.GetValue(3)?.ToString();
            var amount = reader.GetDouble(4);
            var message = reader.GetString(5);
            
            if(string.IsNullOrEmpty(iban) && string.IsNullOrEmpty(vs))
                continue;
            
            var donor = allDonors.FirstOrDefault(d => d.IBAN == iban && d.VariableSymbol == vs);
            if(donor == null)
            {
                donor = new Donor
                {
                    Name = donorName,
                    Email = string.Empty,
                };
                
                if(!string.IsNullOrEmpty(iban)) donor.IBAN = iban;
                if(!string.IsNullOrEmpty(vs)) donor.VariableSymbol = vs;
                
                allDonors.Add(donor);
                unitOfWork.AddForInsert(donor);
            }
            else
            {
                //if(!string.IsNullOrEmpty(iban)) donor.IBAN = iban;
                //if(!string.IsNullOrEmpty(vs)) donor.VariableSymbol = vs;
                //unitOfWork.AddForUpdate(donor);
            }

            unitOfWork.AddForInsert(new Donate
            {
                Donor = donor,
                Amount = amount,
                Date = date,
                Message = message,
                VariableSymbol = vs,
                Iban = iban
            });

        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task ImportDonorsFromExcelAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var allDonors = await donorRepository.GetAllAsync(cancellationToken);
        
        using var reader = ExcelReaderFactory.CreateReader(stream);

        //sheet 0
        reader.Read();
        while (reader.Read())
        {
            var name = reader.GetString(0);
            DateTime? dateOfBirth = reader.IsDBNull(1) ? null : reader.GetDateTime(1);
            var email = reader.GetString(2);
            var iban = reader.GetString(3);
            var vs = reader.GetValue(4).ToString();
            
            if(string.IsNullOrWhiteSpace(name))
                continue;
            
            if(string.IsNullOrWhiteSpace(iban) && string.IsNullOrWhiteSpace(vs))
                continue;
            
            var donor = allDonors.FirstOrDefault(d => d.Name == name 
                                                      && (d.Email == email 
                                                          || d.IBAN == iban 
                                                          || d.VariableSymbol == vs));
            if(donor == null)
            {
                donor = new Donor
                {
                    Name = name,
                    Email = email,
                    IBAN = iban,
                    VariableSymbol = vs,
                    Birthdate = dateOfBirth     
                };
                allDonors.Add(donor);
                unitOfWork.AddForInsert(donor);
            }
            else
            {
                donor.Email = string.IsNullOrWhiteSpace(email) ? donor.Email : email;
                donor.IBAN = string.IsNullOrWhiteSpace(iban) ? donor.IBAN : iban;
                donor.VariableSymbol = string.IsNullOrWhiteSpace(vs) ? donor.VariableSymbol : vs;
                donor.Birthdate = dateOfBirth ?? donor.Birthdate; 
                unitOfWork.AddForUpdate(donor);
            }
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }
    

    public async Task<DataFragmentResult<Donor>> GetDonorsDataFragmentAsync(DonorFilter filter, GridDataProviderRequest<Donor> request,
        CancellationToken cancellationToken = default)
    {
        var data = await donorRepository.GetFragmentAsync(filter, request, cancellationToken);
        return data.ToDataFragmentResult();
    }

    public async Task DeleteDonorAsync(Donor donor, CancellationToken cancellationToken = default)
    {
        unitOfWork.AddForDelete(donor);
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task<Agenda> GetAgendaAsync(CancellationToken cancellationToken = default)
    {
        return await agendaRepository.GetDefaultAsync(cancellationToken);
    }

    public async Task InsertAgendaAsync(Agenda agenda, CancellationToken cancellationToken = default)
    {
        unitOfWork.AddForInsert(agenda);
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task<MailAccount> GetMailAccountAsync(CancellationToken cancellationToken = default)
    {
        return await mailAccountRepository.GetDefaultAsync(cancellationToken);
    }

    public async Task InsertMailAccountAsync(MailAccount mailAccount, CancellationToken cancellationToken = default)
    {
        unitOfWork.AddForInsert(mailAccount);
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public Task UpdateMailAccountAsync(MailAccount mailAccount, CancellationToken cancellationToken = default)
    {
        unitOfWork.AddForUpdate(mailAccount);
        return unitOfWork.CommitAsync(cancellationToken);
    }

    public Task UpdateAgendaAsync(Agenda agenda, CancellationToken cancellationToken = default)
    {
        unitOfWork.AddForUpdate(agenda);
        return unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task<DataFragmentResult<Certificate>> GetCertificatesDataFragmentAsync(CertificateFilter filterModel, GridDataProviderRequest<Certificate> request,
        CancellationToken requestCancellationToken = default)
    {
        var data = await certificateRepository.GetFragmentAsync(filterModel, request, requestCancellationToken);
        return data.ToDataFragmentResult();
    }

    public async Task DeleteCertificateAsync(Certificate cert)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Certificate> CreateCertificateAsync(Donor donor, int year, CancellationToken cancellationToken = default)
    {
        donor.Certificate ??= new Certificate();

        donor.Certificate.Amount = await donateRepository.CalculateDonatesSumAsync(donor.Id, year, cancellationToken);
        donor.Certificate.LastSentDate = null;
        
        unitOfWork.AddForUpdate(donor);
        await unitOfWork.CommitAsync(cancellationToken);

        return donor.Certificate;
    }

    public async Task SaveAgendaAsync(Agenda agenda, CancellationToken cancellationToken = default)
    {
        unitOfWork.AddForUpdate(agenda);
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task SaveCertificateAsync(Certificate cert, CancellationToken cancellationToken = default)
    {
        if(cert.Id == 0)
            unitOfWork.AddForInsert(cert);
        else
            unitOfWork.AddForUpdate(cert);
        
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task SaveDonorAsync(Donor donor, CancellationToken cancellationToken = default)
    {
        if(donor.Id == 0)
            unitOfWork.AddForInsert(donor);
        else
            unitOfWork.AddForUpdate(donor);
        
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task CalculateCertificates(CancellationToken cancellationToken = default)
    {
        unitOfWork.AddRangeForDelete(await certificateRepository.GetAllAsync(cancellationToken));
        await unitOfWork.CommitAsync(cancellationToken);
        
        var allDonates = await donateRepository.GetAllAsync(cancellationToken);
        var allDonors = allDonates.Select(d => d.Donor).Distinct().ToList();

        foreach (var donor in allDonors)
        {
            unitOfWork.AddForInsert(new Certificate()
            {
                Donor = donor,
                Amount = allDonates.Where(d => d.DonorId == donor.Id).Sum(d => d.Amount),
                LastSentDate = null
            });
        }

        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task DeleteCertificatesAsync(IEnumerable<Certificate> list, CancellationToken cancellationToken = default)
    {
        unitOfWork.AddRangeForDelete(list);
        await unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task<IEnumerable<Donor>> GetDonorsWithoutCertificate()
    {
        return await donorRepository.GetAllWithoutCertificate();
    }
}