namespace FabricPlatform.API.Quality.Domain.Model.ValueObjects;

public record InspectionParameters(
    int FabricWidth,
    int FabricWeight,
    string ShadeCondition,
    string WeaveCondition,
    string SurfaceCondition,
    string DefectsDetected,
    string? Observations,
    DateTime UpdatedAt);