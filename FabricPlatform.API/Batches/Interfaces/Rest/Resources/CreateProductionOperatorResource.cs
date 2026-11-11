using System.ComponentModel.DataAnnotations;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record CreateProductionOperatorResource
{
    [Required]
    public string Name { get; init; } = string.Empty;

    [Required]
    public string Specialty { get; init; } = string.Empty;
}
