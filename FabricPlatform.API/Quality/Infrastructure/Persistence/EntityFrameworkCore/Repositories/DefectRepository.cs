namespace FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

public class DefectRepository(AppDbContext context) 
    : BaseRepository<Defect>(context), IDefectRepository
{
    public async Task<IEnumerable<Defect>> FindByBatchIdAsync(string batchId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Defect>()
            .Where(x => x.BatchId == batchId)
            .ToListAsync(cancellationToken);
    }
}