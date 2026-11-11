namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record ObservationResource(int Id, int BatchId, DateTimeOffset Timestamp, string Text, string Author);
