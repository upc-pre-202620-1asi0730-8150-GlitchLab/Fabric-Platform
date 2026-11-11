namespace FabricPlatform.API.Quality.Interfaces.Rest;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FabricPlatform.API.Quality.Application.Internal.Commands;
using FabricPlatform.API.Quality.Application.Internal.CommandHandlers;
using FabricPlatform.API.Quality.Application.Internal.Queries;
using FabricPlatform.API.Quality.Application.Internal.QueryHandlers;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Interfaces.Rest.Resources;

[ApiController]
[Route("api/v1/defects")]
[Tags("Defects")]
public class DefectsController(
    CreateDefectCommandHandler createHandler,
    UpdateDefectCommandHandler updateHandler,
    GetAllDefectsQueryHandler getAllHandler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var defects = await getAllHandler.Handle(new GetAllDefectsQuery(), ct);
        return Ok(defects.Select(MapToResponse));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDefectResource resource, CancellationToken ct)
    {
        var command = new CreateDefectCommand(
            resource.BatchId,
            resource.DefectType,
            resource.Quantity,
            resource.MachineId,
            resource.Origin ?? "Known",
            resource.Status ?? "Pending Decision",
            resource.Observation ?? string.Empty,
            resource.Disposition,
            resource.CorrectionType,
            resource.Date ?? System.DateTime.UtcNow.ToString("o"));

        var result = await createHandler.Handle(command, ct);
        return result.Match<IActionResult>(
            defect => StatusCode(201, MapToResponse(defect)),
            error => BadRequest(error));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDefectResource resource, CancellationToken ct)
    {
        var command = new UpdateDefectCommand(
            id,
            resource.Status,
            resource.Observation,
            resource.Disposition,
            resource.CorrectionType);

        var result = await updateHandler.Handle(command, ct);
        return result.Match<IActionResult>(
            defect => Ok(MapToResponse(defect)),
            error => BadRequest(error));
    }

    private static DefectResponse MapToResponse(Defect d)
    {
        return new DefectResponse(
            d.Id.Value,
            d.BatchId,
            d.DefectType,
            d.Quantity,
            d.MachineId,
            d.Origin,
            d.Status,
            d.Observation,
            d.Disposition,
            d.CorrectionType,
            d.Date);
    }
}