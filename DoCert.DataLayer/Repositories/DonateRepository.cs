using DoCert.Model;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Data.EntityFrameworkCore;
using Havit.Data.EntityFrameworkCore.Patterns.Caching;
using Havit.Data.EntityFrameworkCore.Patterns.QueryServices;
using Havit.Data.EntityFrameworkCore.Patterns.Repositories;
using Havit.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using Havit.Data.Patterns.DataLoaders;
using Havit.Data.Patterns.Infrastructure;
using Havit.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DoCert.DataLayer.Repositories;

public class DonateRepository(
    IDbContext dbContext,
    IEntityKeyAccessor<Donate, int> entityKeyAccessor,
    IDataLoader dataLoader,
    ISoftDeleteManager softDeleteManager,
    IEntityCacheManager entityCacheManager,
    IRepositoryQueryProvider repositoryQueryProvider)
    : DbRepository<Donate>(dbContext, entityKeyAccessor, dataLoader,
        softDeleteManager, entityCacheManager, repositoryQueryProvider), IDonateRepository
{
    public async Task<DataFragment<Donate>> GetFragmentAsync(DonateFilter filter, GridDataProviderRequest<Donate> request,
        CancellationToken cancellationToken = default)
    {
        var filtered = Filter(Data
            .Include(d => d.Donor)
            .Include(d => d.BankAccount)
            , filter);

        var cnt = await filtered.CountAsync(cancellationToken);

        var data = await filtered.ApplyGridDataProviderRequest(request).ToListAsync(cancellationToken: cancellationToken);

        return new DataFragment<Donate>()
        {
            Data = data,
            TotalCount = cnt
        };
    }

    private IQueryable<Donate> Filter(IQueryable<Donate> data, DonateFilter filter)
    {
        return data//.WhereIf(!string.IsNullOrEmpty(filter.DonorName),d => d.Donor.Name.Contains(filter.DonorName, StringComparison.CurrentCultureIgnoreCase))
            .WhereIf(!string.IsNullOrEmpty(filter.DonorName), d =>  EF.Functions.Like(d.Donor.Name, $"%{filter.DonorName}%")) 
            .WhereIf(filter.DateRange.StartDate.HasValue, d => d.Date >= filter.DateRange.StartDate)
            .WhereIf(filter.DateRange.EndDate.HasValue, d => d.Date <= filter.DateRange.EndDate);
    }

    
}
        
        