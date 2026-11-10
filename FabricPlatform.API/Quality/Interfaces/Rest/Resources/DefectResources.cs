namespace FabricPlatform.API.Quality.Interfaces.Rest.Resources;

public record CreateDefectResource(
    string BatchId,
    string DefectType,
    int Quantity,
    string? MachineId,
    string? Origin,
    string? Status,
    string? Observation,
    string? Disposition,
    string? CorrectionType,
    string? Date);

public record UpdateDefectResource(
    string? Status,
    string? Observation,
    string? Disposition,
    string? CorrectionType);

public record DefectResponse(
    int Id,
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