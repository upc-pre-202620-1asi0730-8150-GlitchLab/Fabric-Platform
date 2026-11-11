namespace FabricPlatform.API.Quality.Application.Internal.Commands;

public record CreateDefectCommand(
    string BatchId,
    string DefectType,
    int Quantity,
    string? MachineId,
    string Origin,
    string Status,
    string Observation,
    string? Disposition,
    string? CorrectionType,
    string Date);