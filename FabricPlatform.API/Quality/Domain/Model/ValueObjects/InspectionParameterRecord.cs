namespace FabricPlatform.API.Quality.Domain.Model.ValueObjects;

public record InspectionParameterRecord(
    DateTime Date,
    int FabricWidth,
    int FabricWeight,
    string ShadeCondition,
    string WeaveCondition,
    string SurfaceCondition,
    string DefectsDetected,
    string? Observations);