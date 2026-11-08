using System.ComponentModel.DataAnnotations;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

public record ResumeMachineOperationResource(
    DateTime? EndTime,
    [MaxLength(240, ErrorMessage = "The solution must not exceed 240 characters.")]
    string? Solution);
