namespace FabricPlatform.API.Quality.Interfaces.Rest;

using System;
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
using FabricPlatform.API.Quality.Domain.Model.ValueObjects;
using FabricPlatform.API.Quality.Interfaces.Rest.Resources;

[ApiController]
[Route("api/v1/fabric-inspections")]
[Tags("Fabric-Inspections")]
public class FabricInspectionsController(
    CreateFabricInspectionCommandHandler createHandler,
    UpdateFabricInspectionParametersCommandHandler updateHandler,
    GetAllFabricInspectionsQueryHandler getAllHandler,
    GetFabricInspectionByIdQueryHandler getByIdHandler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await getAllHandler.Handle(new GetAllFabricInspectionsQuery(), ct);
        return Ok(items.Select(MapToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var item = await getByIdHandler.Handle(new GetFabricInspectionByIdQuery(id), ct);
        if (item is null) return NotFound();

        return Ok(MapToResponse(item));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFabricInspectionResource resource, CancellationToken ct)
    {
        var command = new CreateFabricInspectionCommand(
            resource.RollId,
            resource.Supplier,
            resource.InspectionDate,
            resource.Sample,
            resource.Color ?? "Navy",
            resource.InitialResult ?? "Conforming",
            resource.RollStatus ?? "Available for Cutting");

        var result = await createHandler.Handle(command, ct);
        return result.Match<IActionResult>(
            inspection => CreatedAtAction(nameof(GetById), new { id = inspection.Id.Value }, MapToResponse(inspection)),
            error => BadRequest(error));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFabricInspectionResource resource, CancellationToken ct)
    {
        var history = resource.ParameterHistory?.Select(h => new InspectionParameterRecord(
            DateTime.TryParse(h.Date, out var d) ? d : DateTime.UtcNow,
            h.FabricWidth,
            h.FabricWeight,
            h.ShadeCondition,
            h.WeaveCondition,
            h.SurfaceCondition,
            h.DefectsDetected,
            h.Observations));

        var tests = resource.TestHistory?.Select(t => new ShrinkageTestRecord(
            t.TestDate,
            t.Method,
            t.SampleId,
            t.LenShrinkage,
            t.WidthShrinkage,
            t.Result,
            t.Note));

        var command = new UpdateFabricInspectionParametersCommand(
            id,
            resource.Parameters?.FabricWidth,
            resource.Parameters?.FabricWeight,
            resource.Parameters?.ShadeCondition,
            resource.Parameters?.WeaveCondition,
            resource.Parameters?.SurfaceCondition,
            resource.Parameters?.DefectsDetected,
            resource.Parameters?.Observations,
            history,
            resource.ShrinkageTest,
            tests);

        var result = await updateHandler.Handle(command, ct);
        return result.Match<IActionResult>(
            inspection => Ok(MapToResponse(inspection)),
            error => BadRequest(error));
    }

    private static FabricInspectionResponse MapToResponse(FabricInspection item)
    {
        return new FabricInspectionResponse(
            item.Id.Value,
            item.RollId,
            item.Supplier,
            item.InspectionDate,
            item.Sample,
            item.Color,
            item.InitialResult,
            item.RollStatus,
            item.ShrinkageTest,
            item.Parameters is not null ? new ParametersDto(
                item.Parameters.FabricWidth,
                item.Parameters.FabricWeight,
                item.Parameters.ShadeCondition,
                item.Parameters.WeaveCondition,
                item.Parameters.SurfaceCondition,
                item.Parameters.DefectsDetected,
                item.Parameters.Observations) : null,
            item.ParameterHistory.Select(h => new ParameterHistoryDto(
                h.Date.ToString("yyyy-MM-dd"),
                h.FabricWidth,
                h.FabricWeight,
                h.ShadeCondition,
                h.WeaveCondition,
                h.SurfaceCondition,
                h.DefectsDetected,
                h.Observations)),
            item.TestHistory.Select(t => new ShrinkageTestDto(
                t.TestDate,
                t.Method,
                t.SampleId,
                t.LenShrinkage,
                t.WidthShrinkage,
                t.Result,
                t.Note)));
    }
}