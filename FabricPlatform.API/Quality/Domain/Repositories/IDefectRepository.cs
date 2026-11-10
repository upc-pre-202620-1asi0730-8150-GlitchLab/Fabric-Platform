namespace FabricPlatform.API.Quality.Domain.Repositories;

using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Shared.Domain.Repositories;

public interface IDefectRepository : IBaseRepository<Defect>
{
    Task<IEnumerable<Defect>> FindByBatchIdAsync(string batchId, CancellationToken cancellationToken = default);
}