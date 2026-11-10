namespace FabricPlatform.API.Quality.Application.Internal.QueryHandlers;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FabricPlatform.API.Quality.Application.Internal.Queries;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Repositories;

public class GetAllDefectsQueryHandler(IDefectRepository defectRepository)
{
    public async Task<IEnumerable<Defect>> Handle(GetAllDefectsQuery query, CancellationToken ct = default)
    {
        return await defectRepository.ListAsync(ct);
    }
}