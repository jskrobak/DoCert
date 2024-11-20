using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using underware.Data.EntityFrameworkCore;
using underware.Data.EntityFrameworkCore.Patterns.Caching;
using underware.Data.EntityFrameworkCore.Patterns.Repositories;
using underware.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using underware.Data.Patterns.DataLoaders;
using underware.Data.Patterns.Infrastructure;
using underware.Data.Patterns.Repositories.Exceptions;
using underware.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace underware.Data.Patterns.Repositories;

public abstract class DbRepository<TEntity> : IRepository<
#nullable disable
  TEntity> where TEntity : class
{
  internal const int GetObjectsChunkSize = 5000;
  private readonly IDbContext dbContext;
  private readonly IEntityKeyAccessor<TEntity, int> entityKeyAccessor;
  private readonly IRepositoryQueryProvider repositoryQueryProvider;
  private readonly IDataLoader dataLoader;

  /// <summary>
  /// Implementačně jako Lazy, aby kontruktor nevyzvedával DbSet. To umožňuje psát unit testy s mockem dbContextu bez setupu metody Set (dbContext nemusí nic umět).
  /// </summary>
  private readonly Lazy<IDbSet<TEntity>> dbSetLazy;

  private TEntity[] _all;

  /// <summary>
  /// DataLoader pro případné využití v implementaci potomků.
  /// </summary>
  /// <remarks>
  /// Bohužel používáme generovaný constructor v partial třídě, takže není úplně jednoduché si v potomkovi dataLoader odklonit pro vlastní použití.
  /// </remarks>
  protected IDataLoader DataLoader => this.dataLoader;

  /// <summary>DbSet, nad kterým je DbRepository postaven.</summary>
  protected IDbSet<TEntity> DbSet => this.dbSetLazy.Value;

  /// <summary>
  /// Vrací data z datového zdroje jako IQueryable.
  /// Pokud zdroj obsahuje záznamy smazané příznakem, jsou odfiltrovány (nejsou v datech).
  /// </summary>
  protected IQueryable<TEntity> Data
  {
    get
    {
      return this.DbSet.AsQueryable(QueryTagBuilder.CreateTag(this.GetType(), nameof(Data)))
        .WhereNotDeleted<TEntity>(this.SoftDeleteManager);
    }
  }

  /// <summary>
  /// Vrací data z datového zdroje jako IQueryable.
  /// Pokud zdroj obsahuje záznamy smazané příznakem, jsou součástí dat.
  /// </summary>
  protected IQueryable<TEntity> DataIncludingDeleted
  {
    get { return this.DbSet.AsQueryable(QueryTagBuilder.CreateTag(this.GetType(), nameof(DataIncludingDeleted))); }
  }

  /// <summary>SoftDeleteManager používaný repository.</summary>
  protected ISoftDeleteManager SoftDeleteManager { get; }

  /// <summary>EntityCacheManager používaný repository.</summary>
  protected IEntityCacheManager EntityCacheManager { get; }

  /// <summary>Konstruktor.</summary>
  protected DbRepository(
    IDbContext dbContext,
    IEntityKeyAccessor<TEntity, int> entityKeyAccessor,
    IDataLoader dataLoader,
    ISoftDeleteManager softDeleteManager,
    IEntityCacheManager entityCacheManager,
    IRepositoryQueryProvider repositoryQueryProvider)
  {
    Contract.Requires<ArgumentException>(dbContext != null, "dbContext != null");
    Contract.Requires<ArgumentException>(softDeleteManager != null, "softDeleteManager != null");
    this.dbContext = dbContext;
    this.entityKeyAccessor = entityKeyAccessor;
    this.dataLoader = dataLoader;
    this.SoftDeleteManager = softDeleteManager;
    this.EntityCacheManager = entityCacheManager;
    this.repositoryQueryProvider = repositoryQueryProvider;
    this.dbSetLazy = new Lazy<IDbSet<TEntity>>((Func<IDbSet<TEntity>>)(() => dbContext.Set<TEntity>()),
      LazyThreadSafetyMode.None);
  }

  /// <summary>
  /// Vrací objekt dle Id.
  /// Objekt má načtené vlastnosti definované v metodě GetLoadReferences.
  /// </summary>
  /// <exception cref="T:Havit.Data.Patterns.Exceptions.ObjectNotFoundException">Objekt s daným Id nebyl nalezen.</exception>
  public TEntity GetObject(int id)
  {
    Contract.Requires<ArgumentException>(id != 0, "id != default(int)");
    TEntity entity = this.DbSet.FindTracked((object)id);
    if ((object)entity == null)
      this.EntityCacheManager.TryGetEntity<TEntity>((object)id, out entity);
    if ((object)entity == null)
    {
      entity =
        this.repositoryQueryProvider.GetGetObjectCompiledQuery<TEntity>(this.GetType(), this.entityKeyAccessor)(
          (DbContext)this.dbContext, id);
      if ((object)entity != null)
        this.EntityCacheManager.StoreEntity<TEntity>(entity);
    }

    if ((object)entity == null)
      this.ThrowObjectNotFoundException(id);
    this.LoadReferences(entity);
    return entity;
  }

  /// <summary>
  /// Vrací objekt dle Id.
  /// Objekt má načtené vlastnosti definované v metodě GetLoadReferences.
  /// </summary>
  /// <exception cref="T:Havit.Data.Patterns.Exceptions.ObjectNotFoundException">Objekt s daným Id nebyl nalezen.</exception>
  public async Task<TEntity> GetObjectAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
  {
    DbRepository<TEntity> dbRepository = this;
    Contract.Requires<ArgumentException>(id != 0, "id != default(int)");
    TEntity result = dbRepository.DbSet.FindTracked((object)id);
    if ((object)result == null)
      dbRepository.EntityCacheManager.TryGetEntity<TEntity>((object)id, out result);
    if ((object)result == null)
    {
      result =
        await dbRepository.repositoryQueryProvider.GetGetObjectAsyncCompiledQuery<TEntity>(dbRepository.GetType(),
            dbRepository.entityKeyAccessor)((DbContext)dbRepository.dbContext, id, cancellationToken)
          .ConfigureAwait(false);
      if ((object)result != null)
        dbRepository.EntityCacheManager.StoreEntity<TEntity>(result);
    }

    if ((object)result == null)
      dbRepository.ThrowObjectNotFoundException(id);
    await dbRepository.LoadReferencesAsync(new TEntity[1]
    {
      result
    }, cancellationToken).ConfigureAwait(false);
    TEntity objectAsync = result;
    result = default(TEntity);
    return objectAsync;
  }

  /// <summary>
  /// Vrací instance objektů dle Id.
  /// Vrací instance objektů dle Id.
  /// Objekty mají načtené vlastnosti definované v metodě GetLoadReferences.
  /// </summary>
  /// <exception cref="T:Havit.Data.Patterns.Exceptions.ObjectNotFoundException">Alespoň jeden objekt nebyl nalezen.</exception>
  public List<TEntity> GetObjects(params int[] ids)
  {
    Contract.Requires(ids != null, "ids != null");
    HashSet<TEntity> collection = new HashSet<TEntity>();
    HashSet<int> intSet = new HashSet<int>();
    foreach (int id in ids)
    {
      Contract.Assert<ArgumentException>(id != 0, "id != default(int)");
      TEntity tracked = this.DbSet.FindTracked((object)id);
      if ((object)tracked != null)
      {
        collection.Add(tracked);
      }
      else
      {
        TEntity entity;
        if (this.EntityCacheManager.TryGetEntity<TEntity>((object)id, out entity))
          collection.Add(entity);
        else
          intSet.Add(id);
      }
    }

    List<TEntity> objects = new List<TEntity>(ids.Length);
    objects.AddRange((IEnumerable<TEntity>)collection);
    if (intSet.Count > 0)
    {
      Func<DbContext, int[], IEnumerable<TEntity>> objectsCompiledQuery =
        this.repositoryQueryProvider.GetGetObjectsCompiledQuery<TEntity>(this.GetType(), this.entityKeyAccessor);
      List<TEntity> entityList;
      if (intSet.Count <= 5000)
      {
        entityList = objectsCompiledQuery((DbContext)this.dbContext, intSet.ToArray<int>()).ToList<TEntity>();
      }
      else
      {
        entityList = new List<TEntity>();
        foreach (int[] numArray in EnumerableExt.Chunkify<int>((IEnumerable<int>)intSet, 5000))
          entityList.AddRange((IEnumerable<TEntity>)objectsCompiledQuery((DbContext)this.dbContext, numArray)
            .ToList<TEntity>());
      }

      if (intSet.Count != entityList.Count)
        this.ThrowObjectNotFoundException(intSet
          .Except<int>(
            entityList.Select<TEntity, int>(new Func<TEntity, int>(this.entityKeyAccessor.GetEntityKeyValue)))
          .ToArray<int>());
      foreach (TEntity entity in entityList)
        this.EntityCacheManager.StoreEntity<TEntity>(entity);
      objects.AddRange((IEnumerable<TEntity>)entityList);
    }

    this.LoadReferences(objects.ToArray());
    return objects;
  }

  /// <summary>
  /// Vrací instance objektů dle Id.
  /// Objekty mají načtené vlastnosti definované v metodě GetLoadReferences.
  /// </summary>
  /// <exception cref="T:Havit.Data.Patterns.Exceptions.ObjectNotFoundException">Alespoň jeden objekt nebyl nalezen.</exception>
  public async Task<List<TEntity>> GetObjectsAsync(int[] ids,
    CancellationToken cancellationToken = default(CancellationToken))
  {
    DbRepository<TEntity> dbRepository = this;
    Contract.Requires(ids != null, "ids != null");
    HashSet<TEntity> collection = new HashSet<TEntity>();
    HashSet<int> idsToLoad = new HashSet<int>();
    foreach (int id in ids)
    {
      Contract.Assert<ArgumentException>(id != 0, "id != default(int)");
      TEntity tracked = dbRepository.DbSet.FindTracked((object)id);
      if ((object)tracked != null)
      {
        collection.Add(tracked);
      }
      else
      {
        TEntity entity;
        if (dbRepository.EntityCacheManager.TryGetEntity<TEntity>((object)id, out entity))
          collection.Add(entity);
        else
          idsToLoad.Add(id);
      }
    }

    List<TEntity> result = new List<TEntity>(ids.Length);
    result.AddRange((IEnumerable<TEntity>)collection);
    if (idsToLoad.Count > 0)
    {
      Func<DbContext, int[], IAsyncEnumerable<TEntity>> query =
        dbRepository.repositoryQueryProvider.GetGetObjectsAsyncCompiledQuery<TEntity>(dbRepository.GetType(),
          dbRepository.entityKeyAccessor);
      List<TEntity> loadedObjects;
      if (idsToLoad.Count <= 5000)
      {
        loadedObjects = await AsyncEnumerable
          .ToListAsync<TEntity>(query((DbContext)dbRepository.dbContext, idsToLoad.ToArray<int>()), cancellationToken)
          .ConfigureAwait(false);
      }
      else
      {
        loadedObjects = new List<TEntity>();
        foreach (int[] numArray in EnumerableExt.Chunkify<int>((IEnumerable<int>)idsToLoad, 5000))
        {
          List<TEntity> entityList = loadedObjects;
          entityList.AddRange((IEnumerable<TEntity>)await AsyncEnumerable
            .ToListAsync<TEntity>(query((DbContext)dbRepository.dbContext, numArray), cancellationToken)
            .ConfigureAwait(false));
          entityList = (List<TEntity>)null;
        }
      }

      if (idsToLoad.Count != loadedObjects.Count)
      {
        int[] array = idsToLoad
          .Except<int>(
            loadedObjects.Select<TEntity, int>(
              new Func<TEntity, int>(dbRepository.entityKeyAccessor.GetEntityKeyValue))).ToArray<int>();
        dbRepository.ThrowObjectNotFoundException(array);
      }

      foreach (TEntity entity in loadedObjects)
        dbRepository.EntityCacheManager.StoreEntity<TEntity>(entity);
      result.AddRange((IEnumerable<TEntity>)loadedObjects);
      query = (Func<DbContext, int[], IAsyncEnumerable<TEntity>>)null;
      loadedObjects = (List<TEntity>)null;
    }

    await dbRepository.LoadReferencesAsync(result.ToArray(), cancellationToken).ConfigureAwait(false);
    List<TEntity> objectsAsync = result;
    idsToLoad = (HashSet<int>)null;
    result = (List<TEntity>)null;
    return objectsAsync;
  }

  /// <summary>
  /// Vrací seznam všech (příznakem nesmazaných) objektů typu TEntity.
  /// Dotaz na seznam objektů provede jednou, při opakovaném volání vrací data z paměti.
  /// Objekty mají načtené vlastnosti definované v metodě GetLoadReferences.
  /// </summary>
  public List<TEntity> GetAll()
  {
    if (this._all == null)
    {
      object keys;
      TEntity[] array;
      if (this.EntityCacheManager.TryGetAllKeys<TEntity>(out keys))
      {
        array = this.GetObjects((int[])keys).ToArray();
      }
      else
      {
        array =
          this.repositoryQueryProvider.GetGetAllCompiledQuery<TEntity>(this.GetType(), this.SoftDeleteManager)(
            (DbContext)this.dbContext).ToArray<TEntity>();
        this.EntityCacheManager.StoreAllKeys<TEntity>((object)((IEnumerable<TEntity>)array)
          .Select<TEntity, int>((Func<TEntity, int>)(entity => this.entityKeyAccessor.GetEntityKeyValue(entity)))
          .ToArray<int>());
        foreach (TEntity entity in array)
          this.EntityCacheManager.StoreEntity<TEntity>(entity);
      }

      this.LoadReferences(array);
      this._all = array;
      this.dbContext.RegisterAfterSaveChangesAction((Action)(() => this._all = (TEntity[])null));
    }

    return new List<TEntity>((IEnumerable<TEntity>)this._all);
  }

  /// <summary>
  /// Vrací seznam všech (příznakem nesmazaných) objektů typu TEntity.
  /// Dotaz na seznam objektů provede jednou, při opakovaném volání vrací data z paměti.
  /// Objekty mají načtené vlastnosti definované v metodě GetLoadReferences.
  /// </summary>
  public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
  {
    throw new NotImplementedException();
  }

  /// <summary>
  /// Zajistí načtení vlastností definovaných v meodě GetLoadReferences.
  /// </summary>
  /// <remarks>
  /// Metodu lze overridovat, pokud chceme doplnit podrobnější implementaci dočítání (přes IDataLoader), např. nepodporované dočítání prvků v kolekcích.
  /// Nezapomeňte však overridovat synchronní i asynchronní verzi! Jsou to nezávislé implementace...
  /// </remarks>
  protected virtual void LoadReferences(params TEntity[] entities)
  {
    Contract.Requires(entities != null, "entities != null");
    Expression<Func<TEntity, object>>[] array = this.GetLoadReferences().ToArray<Expression<Func<TEntity, object>>>();
    if (!((IEnumerable<Expression<Func<TEntity, object>>>)array).Any<Expression<Func<TEntity, object>>>())
      return;
    this.dataLoader.LoadAll<TEntity>((IEnumerable<TEntity>)entities, array);
  }

  /// <summary>
  /// Zajistí načtení vlastností definovaných v meodě GetLoadReferences.
  /// </summary>
  /// <remarks>
  /// Metodu lze overridovat, pokud chceme doplnit podrobnější implementaci dočítání (přes IDataLoader), např. nepodporované dočítání prvků v kolekcích.
  /// Nezapomeňte však overridovat synchronní i asynchronní verzi! Jsou to nezávislé implementace...
  /// </remarks>
  protected virtual async Task LoadReferencesAsync(
    TEntity[] entities,
    CancellationToken cancellationToken = default(CancellationToken))
  {
    Contract.Requires(entities != null, "entities != null");
    Expression<Func<TEntity, object>>[] array = this.GetLoadReferences().ToArray<Expression<Func<TEntity, object>>>();
    if (!((IEnumerable<Expression<Func<TEntity, object>>>)array).Any<Expression<Func<TEntity, object>>>())
      return;
    await this.dataLoader.LoadAllAsync<TEntity>((IEnumerable<TEntity>)entities, array, cancellationToken)
      .ConfigureAwait(false);
  }

  /// <summary>
  /// Vrací expressions určující, které vlastnosti budou s objektem načteny.
  /// Načítání prování DbDataLoader.
  /// </summary>
  protected virtual IEnumerable<Expression<Func<TEntity, object>>> GetLoadReferences()
  {
    return Enumerable.Empty<Expression<Func<TEntity, object>>>();
  }

  private void ThrowObjectNotFoundException(params int[] missingIds)
  {
    Contract.Requires(missingIds != null, "missingIds != null");
    Contract.Requires(missingIds.Length != 0, "missingIds.Length > 0");
    throw new ObjectNotFoundException(missingIds.Length == 1
      ? string.Format("Object {0} with key {1} not found.", (object)typeof(TEntity).Name, (object)missingIds[0])
      : string.Format("Objects {0} with keys {1} not found.", (object)typeof(TEntity).Name,
        (object)string.Join(", ",
          ((IEnumerable<int>)missingIds).Select<int, string>((Func<int, string>)(item => item.ToString())))));
  }
}