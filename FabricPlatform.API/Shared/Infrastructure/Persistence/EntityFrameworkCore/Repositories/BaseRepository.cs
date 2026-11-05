using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Shared.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Base repository for all repositories
/// </summary>
/// <remarks>
///     This class is used to define the basic CRUD operations for all repositories.
///     This class implements the IBaseRepository interface.
/// </remarks>
/// <typeparam name="TEntity">
///     The entity type for the repository
/// </typeparam>
public class BaseRepository<TEntity>(AppDbContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context = context;

    // inheritedDoc
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    // inheritedDoc
    public virtual async Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FindAsync([ToKeyValue(id)], cancellationToken);
    }

    // inheritedDoc
    public virtual async Task<TEntity?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindByIdAsync(id, cancellationToken);
        if (entity is not null) Context.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    // inheritedDoc
    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    // inheritedDoc
    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    // inheritedDoc
    public virtual async Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    // inheritedDoc
    public virtual async Task<IEnumerable<TEntity>> ListAsNoTrackingAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     Converts an integer id into the CLR type of the entity's primary key (e.g. a strongly typed id).
    /// </summary>
    private object ToKeyValue(int id)
    {
        var keyType = Context.Model.FindEntityType(typeof(TEntity))!.FindPrimaryKey()!.Properties[0].ClrType;
        return keyType == typeof(int) ? id : Activator.CreateInstance(keyType, id)!;
    }
}
