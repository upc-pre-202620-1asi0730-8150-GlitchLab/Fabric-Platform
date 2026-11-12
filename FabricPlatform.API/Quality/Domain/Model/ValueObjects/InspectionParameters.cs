namespace FabricPlatform.API.Quality.Domain.Model.ValueObjects;

public record InspectionParameters(
    decimal FabricWidth,
    int? FabricWeight,
    decimal? FabricLength,
    string ShadeCondition,
    string WeaveCondition,
    string SurfaceCondition,
    string DefectsDetected,
    string? Observations,
    DateTime UpdatedAt);