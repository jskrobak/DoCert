using DoCert.Model;
using Havit.Data.EntityFrameworkCore;
using Havit.Data.EntityFrameworkCore.Patterns.Caching;
using Havit.Data.EntityFrameworkCore.Patterns.QueryServices;
using Havit.Data.EntityFrameworkCore.Patterns.Repositories;
using Havit.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using Havit.Data.Patterns.DataLoaders;
using Havit.Data.Patterns.Infrastructure;
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
    public Task<DataFragment<Donor>> GetFragmentAsync(DonorFilter filter, int start, int? count, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Donor?> GetDonorByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Data.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }
}