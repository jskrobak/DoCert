using DoCert.Model;
using Havit.Data.EntityFrameworkCore;
using Havit.Data.EntityFrameworkCore.Patterns.Caching;
using Havit.Data.EntityFrameworkCore.Patterns.Repositories;
using Havit.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using Havit.Data.Patterns.DataLoaders;
using Havit.Data.Patterns.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DoCert.DataLayer.Repositories;

public class AgendaRepository(
    IDbContext dbContext,
    IEntityKeyAccessor<Agenda, int> entityKeyAccessor,
    IDataLoader dataLoader,
    ISoftDeleteManager softDeleteManager,
    IEntityCacheManager entityCacheManager,
    IRepositoryQueryProvider repositoryQueryProvider)
    : DbRepository<Agenda>(dbContext, entityKeyAccessor, dataLoader,
        softDeleteManager, entityCacheManager, repositoryQueryProvider), IAgendaRepository
{
    public async Task<Agenda> GetDefaultAsync(CancellationToken cancellationToken = default)
    {
        return await Data.Include(a => a.MailAccount).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}