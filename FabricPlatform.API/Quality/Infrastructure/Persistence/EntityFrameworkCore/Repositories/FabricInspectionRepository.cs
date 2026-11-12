namespace FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

public class FabricInspectionRepository(AppDbContext context)
    : BaseRepository<FabricInspection>(context), IFabricInspectionRepository
{
    public async Task<FabricInspection?> FindByRollIdAsync(string rollId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<FabricInspection>()
            .FirstOrDefaultAsync(x => x.RollId == rollId, cancellationToken);
    }
}