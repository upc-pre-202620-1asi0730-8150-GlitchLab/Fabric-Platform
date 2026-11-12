namespace FabricPlatform.API.Quality.Application.Internal.Commands;

using System.Collections.Generic;
using FabricPlatform.API.Quality.Domain.Model.ValueObjects;

public record UpdateFabricInspectionParametersCommand(
    int InspectionId,
    decimal? FabricWidth,
    int? FabricWeight,
    decimal? FabricLength,
    string? ShadeCondition,
    string? WeaveCondition,
    string? SurfaceCondition,
    string? DefectsDetected,
    string? Observations,
    IEnumerable<InspectionParameterRecord>? ParameterHistory,
    string? ShrinkageTest,
    IEnumerable<ShrinkageTestRecord>? TestHistory);