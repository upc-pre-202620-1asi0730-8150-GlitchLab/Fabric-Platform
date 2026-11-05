using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FabricPlatform.API.Shared.Domain.Repositories;

/// <summary>
///     Base repository interface for all repositories
/// </summary>
/// <remarks>
///     This interface is used to define the basic CRUD operations for all repositories
/// </remarks>
/// <typeparam name="TEntity">
///     The entity type for the repository
/// </typeparam>
public interface IBaseRepository<TEntity>
{
    /// <summary>
    ///     Add an entity to the repository
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find an entity by its id
    /// </summary>
    Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find an entity by its id without change tracking
    /// </summary>
    Task<TEntity?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update an entity in the repository
    /// </summary>
    void Update(TEntity entity);

    /// <summary>
    ///     Remove an entity from the repository
    /// </summary>
    void Remove(TEntity entity);

    /// <summary>
    ///     List all entities in the repository
    /// </summary>
    Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     List all entities in the repository without change tracking
    /// </summary>
    Task<IEnumerable<TEntity>> ListAsNoTrackingAsync(CancellationToken cancellationToken = default);
}
