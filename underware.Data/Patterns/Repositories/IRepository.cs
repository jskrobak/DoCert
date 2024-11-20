﻿namespace underware.Data.Patterns.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>Vrací instanci objektu dle Id.</summary>
    /// <exception cref="T:underware.Data.Patterns.Exceptions.ObjectNotFoundException">Objekt s daným Id nebyl nalezen.</exception>
    TEntity GetObject(int id);

    /// <summary>Vrací instanci objektu dle Id.</summary>
    /// <exception cref="T:underware.Data.Patterns.Exceptions.ObjectNotFoundException">Objekt s daným Id nebyl nalezen.</exception>
    Task<TEntity> GetObjectAsync(int id, CancellationToken cancellationToken = default (CancellationToken));

    /// <summary>Vrací instance objektů dle Id.</summary>
    List<TEntity> GetObjects(params int[] ids);

    /// <summary>Vrací instance objektů dle Id.</summary>
    Task<List<TEntity>> GetObjectsAsync(int[] ids, CancellationToken cancellationToken = default (CancellationToken));

    /// <summary>
    /// Vrací seznam všech (příznakem nesmazaných) objektů typu TEntity.
    /// </summary>
    List<TEntity> GetAll();

    /// <summary>
    /// Vrací seznam všech (příznakem nesmazaných) objektů typu TEntity.
    /// </summary>
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default (CancellationToken));
}