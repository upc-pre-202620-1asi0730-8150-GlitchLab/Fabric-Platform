using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record AssignOperatorResource
{
    [Required]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int BatchId { get; init; }

    [Required]
    [JsonPropertyName("operatorName")]
    public string OperatorName { get; init; } = string.Empty;
}