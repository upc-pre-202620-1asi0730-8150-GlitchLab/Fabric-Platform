namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record UpdateTraceabilityMovementResource(
    string? Stage,
    [property: System.Text.Json.Serialization.JsonConverter(typeof(FlexibleDateTimeOffsetConverter))] DateTimeOffset? DateTime,
    int? QtyIn,
    int? QtyOut,
    int? Difference,
    string? Responsible,
    string? Status);
