namespace FabricPlatform.API.Quality.Application.Internal.Commands;

public record UpdateDefectCommand(
    int DefectId,
    string? Status,
    string? Observation,
    string? Disposition,
    string? CorrectionType);