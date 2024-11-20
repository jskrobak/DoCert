using DoCert.Contracts;
using DoCert.DataLayer;
using DoCert.DataLayer.Repositories;
using DoCert.Model;
using Havit.Data.EntityFrameworkCore.Patterns.QueryServices;
using Havit.Data.EntityFrameworkCore.Patterns.UnitOfWorks;
using Havit.Data.Patterns.Repositories;
using Havit.Data.Patterns.UnitOfWorks;

namespace DoCert.Services;

public class DataService(IDonateRepository donateRepository, IUnitOfWork unitOfWork): IDataService
{
    public async Task<DataFragmentResult<Donate>> GetDonatesAsync(int start, int? count, CancellationToken cancellationToken = default)
    {
        var data = await donateRepository.GetFragmentAsync(start, count, cancellationToken);
        return data.ToDataFragmentResult();
    }

    public async Task InsertDonatesAsync(List<Donate> donates, CancellationToken cancellationToken = default)
    {   
        foreach(var donate in donates)
            unitOfWork.AddForInsert(donate);

        await unitOfWork.CommitAsync(cancellationToken);
    }
}