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
    public async Task<DataFragment<Donate>> GetFragmentAsync(int start, int? count,
        CancellationToken cancellationToken = default)
    {
        var cnt = await Data.CountAsync(cancellationToken);
        var data = await Data.Include(d => d.Donor)
            .Include(d => d.BankAccount)
            .Skip(start).Take(count ?? 20).ToListAsync();
        
        return new DataFragment<Donate>()
        {
            Data = data,
            TotalCount = cnt
        };
    }



}
        
        