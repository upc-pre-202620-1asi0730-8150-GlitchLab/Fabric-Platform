namespace FabricPlatform.API.Quality.Application.Internal.Commands;

public record CreateFabricInspectionCommand(
    string RollId,
    string Supplier,
    string InspectionDate,
    string Sample,
    string Color,
    string InitialResult,
    string RollStatus,
    decimal? RollLength,
    string ShadeMatch,
    string WeaveCondition,
    string Observation);