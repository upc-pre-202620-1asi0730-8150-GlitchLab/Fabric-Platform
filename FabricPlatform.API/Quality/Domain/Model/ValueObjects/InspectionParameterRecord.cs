namespace FabricPlatform.API.Quality.Domain.Model.ValueObjects;

using System;

public record InspectionParameterRecord(
    DateTime Date,
    decimal FabricWidth,
    int? FabricWeight,
    decimal? FabricLength,
    string ShadeCondition,
    string WeaveCondition,
    string SurfaceCondition,
    string DefectsDetected,
    string? Observations);