using Microsoft.EntityFrameworkCore;
using underware.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using underware.Data.Patterns.Infrastructure;

namespace underware.Data.EntityFrameworkCore.Patterns.Repositories;

  /// <summary>Poskytuje dotazy pro použití v repository.</summary>
  public interface IRepositoryQueryProvider
  {
    /// <summary>
    /// Vrátí dotaz pro metodu <see cref="M:Havit.Data.EntityFrameworkCore.Patterns.Repositories.DbRepository`1.GetObject(System.Int32)" />.
    /// </summary>
    Func<DbContext, int, TEntity> GetGetObjectCompiledQuery<TEntity>(
      Type repositoryType,
      IEntityKeyAccessor<TEntity, int> entityKeyAccessor)
      where TEntity : class;

    /// <summary>
    /// Vrátí dotaz pro metodu <see cref="M:Havit.Data.EntityFrameworkCore.Patterns.Repositories.DbRepository`1.GetObjectAsync(System.Int32,System.Threading.CancellationToken)" />.
    /// </summary>
    Func<DbContext, int, CancellationToken, Task<TEntity>> GetGetObjectAsyncCompiledQuery<TEntity>(
      Type repositoryType,
      IEntityKeyAccessor<TEntity, int> entityKeyAccessor)
      where TEntity : class;

    /// <summary>
    /// Vrátí dotaz pro metodu <see cref="M:Havit.Data.EntityFrameworkCore.Patterns.Repositories.DbRepository`1.GetObjects(System.Int32[])" />.
    /// </summary>
    Func<DbContext, int[], IEnumerable<TEntity>> GetGetObjectsCompiledQuery<TEntity>(
      Type repositoryType,
      IEntityKeyAccessor<TEntity, int> entityKeyAccessor)
      where TEntity : class;

    /// <summary>
    /// Vrátí dotaz pro metodu <see cref="M:Havit.Data.EntityFrameworkCore.Patterns.Repositories.DbRepository`1.GetObjectsAsync(System.Int32[],System.Threading.CancellationToken)" />.
    /// </summary>
    Func<DbContext, int[], IAsyncEnumerable<TEntity>> GetGetObjectsAsyncCompiledQuery<TEntity>(
      Type repositoryType,
      IEntityKeyAccessor<TEntity, int> entityKeyAccessor)
      where TEntity : class;

    /// <summary>
    /// Vrátí dotaz pro metodu <see cref="M:Havit.Data.EntityFrameworkCore.Patterns.Repositories.DbRepository`1.GetAll" />.
    /// </summary>
    Func<DbContext, IEnumerable<TEntity>> GetGetAllCompiledQuery<TEntity>(
      Type repositoryType,
      ISoftDeleteManager softDeleteManager)
      where TEntity : class;

    /// <summary>
    /// Vrátí dotaz pro metodu <see cref="M:Havit.Data.EntityFrameworkCore.Patterns.Repositories.DbRepository`1.GetAllAsync(System.Threading.CancellationToken)" />.
    /// </summary>
    Func<DbContext, IAsyncEnumerable<TEntity>> GetGetAllAsyncCompiledQuery<TEntity>(
      Type repositoryType,
      ISoftDeleteManager softDeleteManager)
      where TEntity : class;
  }