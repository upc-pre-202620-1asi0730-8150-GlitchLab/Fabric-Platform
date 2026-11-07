namespace FabricPlatform.API.Quality.Domain.Repositories;

using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Shared.Domain.Repositories;

public interface IFabricInspectionRepository : IBaseRepository<FabricInspection>
{
    Task<FabricInspection?> FindByRollIdAsync(string rollId, CancellationToken cancellationToken = default);
}