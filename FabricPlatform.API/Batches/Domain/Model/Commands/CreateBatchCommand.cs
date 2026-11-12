namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public record CreateBatchCommand(string GarmentModel, int ProjectedQuantity, DateTimeOffset DeliveryDate, string? Notes);