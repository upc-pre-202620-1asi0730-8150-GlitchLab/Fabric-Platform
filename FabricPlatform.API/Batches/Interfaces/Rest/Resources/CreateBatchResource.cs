using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record CreateBatchResource
{
    [Required]
    public string? GarmentModel { get; init; }

    [Required]
    public int? ProjectedQuantity { get; init; }

    [Required]
    [JsonConverter(typeof(FlexibleDateTimeOffsetConverter))]
    public DateTimeOffset? DeliveryDate { get; init; }

    public string? Notes { get; init; }
}
