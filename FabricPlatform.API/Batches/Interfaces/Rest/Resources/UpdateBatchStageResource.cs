using System.ComponentModel.DataAnnotations;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record UpdateBatchStageResource
{
    [Required] public string? CurrentStage { get; init; }

    [Range(0, int.MaxValue)] public int? ProcessedQuantity { get; init; }

    public string? Notes { get; init; }
}