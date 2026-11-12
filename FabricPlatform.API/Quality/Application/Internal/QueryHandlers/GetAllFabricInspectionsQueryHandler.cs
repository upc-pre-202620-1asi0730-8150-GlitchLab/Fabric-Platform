namespace FabricPlatform.API.Quality.Application.Internal.QueryHandlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Application.Internal.Queries;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;

public class GetAllFabricInspectionsQueryHandler(IFabricInspectionRepository inspectionRepository)
{
    public async Task<IEnumerable<FabricInspection>> Handle(GetAllFabricInspectionsQuery query, CancellationToken ct = default)
    {
        return await inspectionRepository.ListAsync(ct);
    }
}