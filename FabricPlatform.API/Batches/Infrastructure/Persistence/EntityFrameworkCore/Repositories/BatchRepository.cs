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

    public async Task<Batch?> FindByIdAsync(BatchId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Batch>()
            .Include(b => b.Stages)
            .Include(b => b.Operators)
            .Include(b => b.Movements)
            .Include(b => b.Observations)
            .FirstOrDefaultAsync(b => b.Id.Equals(id), cancellationToken);
    }

    public async Task<Batch?> FindByCodeAsync(BatchCode code, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Batch>()
            .Include(b => b.Stages)
            .Include(b => b.Operators)
            .Include(b => b.Movements)
            .Include(b => b.Observations)
            .FirstOrDefaultAsync(b => b.Code.Equals(code), cancellationToken);
    }

    public async Task<IEnumerable<Batch>> ListByStatusAsync(BatchStatus status, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Batch>()
            .Where(b => b.Status == status)
            .Include(b => b.Stages)
            .ToListAsync(cancellationToken);
    }
}