namespace FabricPlatform.API.Quality.Application.Internal.QueryHandlers;

using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Application.Internal.Queries;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;

public class GetFabricInspectionByIdQueryHandler(IFabricInspectionRepository inspectionRepository)
{
    public async Task<FabricInspection?> Handle(GetFabricInspectionByIdQuery query, CancellationToken ct = default)
    {
        return await inspectionRepository.FindByIdAsync(query.Id, ct);
    }
}