using Microsoft.EntityFrameworkCore;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Batches.Domain.Repositories;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class BatchRepository : BaseRepository<Batch>, IBatchRepository
{
    public BatchRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Batch?> FindByIdAsync(BatchId batchId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Batch>()
            .Include(b => b.Stages)
            .Include(b => b.Operators)
            .Include(b => b.Movements)
            .Include(b => b.Observations)
            .FirstOrDefaultAsync(b => b.Id == batchId, cancellationToken); // <--- Comparación directa gracias al ValueConverter
    }

    public async Task<Batch?> FindByCodeAsync(BatchCode batchCode, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Batch>()
            .Include(b => b.Stages)
            .Include(b => b.Operators)
            .Include(b => b.Movements)
            .Include(b => b.Observations)
            .FirstOrDefaultAsync(b => b.Code == batchCode, cancellationToken);
    }

    public async Task<Batch?> FindByMovementIdAsync(int movementId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Batch>()
            .Include(batch => batch.Movements)
            .FirstOrDefaultAsync(batch => batch.Movements.Any(movement => movement.Id == movementId), cancellationToken);
    }

    public new async Task<IEnumerable<Batch>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Batch>()
            .Include(b => b.Stages)
            .Include(b => b.Operators)
            .Include(b => b.Movements)
            .Include(b => b.Observations)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Batch>> ListByStatusAsync(BatchStatus status, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Batch>()
            .Where(b => b.Status == status)
            .Include(b => b.Stages)
            .Include(b => b.Operators)
            .Include(b => b.Movements)
            .Include(b => b.Observations)
            .ToListAsync(cancellationToken);
    }
}