using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record CreateObservationResource
{
    [Required]
    [Range(1, int.MaxValue)]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int BatchId { get; init; }

    [Required]
    [MaxLength(500)]
    [JsonPropertyName("text")]
    public string Text { get; init; } = string.Empty;
}