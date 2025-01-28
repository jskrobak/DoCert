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
using Microsoft.EntityFrameworkCore;

namespace DoCert.DataLayer.Repositories;

public class DonorRepository(
    IDbContext dbContext,
    IEntityKeyAccessor<Donor, int> entityKeyAccessor,
    IDataLoader dataLoader,
    ISoftDeleteManager softDeleteManager,
    IEntityCacheManager entityCacheManager,
    IRepositoryQueryProvider repositoryQueryProvider)
    : DbRepository<Donor>(dbContext, entityKeyAccessor, dataLoader, softDeleteManager, entityCacheManager,
        repositoryQueryProvider), IDonorRepository
{
    public async Task<DataFragment<Donor>> GetFragmentAsync(DonorFilter filter, GridDataProviderRequest<Donor> request,
        CancellationToken cancellationToken = default)
    {
        var filtered = Filter(Data, filter);
        //filtered = filtered.Include(d => d.Certificates);

        var cnt = await filtered.CountAsync(cancellationToken);

        var data = await filtered.ApplyGridDataProviderRequest<Donor>(request).ToListAsync(cancellationToken: cancellationToken);

        return new DataFragment<Donor>()
        {
            Data = data,
            TotalCount = cnt
        };
    }

    private IQueryable<Donor> Filter(IQueryable<Donor> data, DonorFilter filter)
    {
        return data
            .WhereIf(!string.IsNullOrEmpty(filter.Name), d => EF.Functions.Like(d.Name, $"%{filter.Name}%"));
    }

    public async Task<Donor?> GetDonorByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Data.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Donor>> GetAllWithoutCertificate()
    {
        return await Data
            .Include(d => d.Certificate)
            .Where(d => d.Certificate==null).ToListAsync(); 
    }
}