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

public class CertificateRepository(
    IDbContext dbContext,
    IEntityKeyAccessor<Certificate, int> entityKeyAccessor,
    IDataLoader dataLoader,
    ISoftDeleteManager softDeleteManager,
    IEntityCacheManager entityCacheManager,
    IRepositoryQueryProvider repositoryQueryProvider)
    : DbRepository<Certificate>(dbContext, entityKeyAccessor, dataLoader, softDeleteManager, entityCacheManager,
        repositoryQueryProvider), ICertificateRepository
{
    public async Task<DataFragment<Certificate>> GetFragmentAsync(CertificateFilter filter, GridDataProviderRequest<Certificate> request,
        CancellationToken cancellationToken = default)
    {
        var filtered = Filter(Data
                .Include(c => c.Donor)
            , filter);

        var cnt = await filtered.CountAsync(cancellationToken);

        var data = await filtered.ApplyGridDataProviderRequest(request).ToListAsync(cancellationToken: cancellationToken);

        return new DataFragment<Certificate>()
        {
            Data = data,
            TotalCount = cnt
        };
    }

    public Task<bool> AnyAsync()
    {
        return Data.AnyAsync();
    }

    private IQueryable<Certificate> Filter(IQueryable<Certificate> data, CertificateFilter filter)
    {
        return
            data //.WhereIf(!string.IsNullOrEmpty(filter.DonorName),d => d.Donor.Name.Contains(filter.DonorName, StringComparison.CurrentCultureIgnoreCase))
                .WhereIf(!string.IsNullOrEmpty(filter.DonorName),
                    d => EF.Functions.Like(d.Donor.Name, $"%{filter.DonorName}%"));
        //.WhereIf(filter.Years.Count != 0, d => filter.Years.Contains(d.Year));
    }
}