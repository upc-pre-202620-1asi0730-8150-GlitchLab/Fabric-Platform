namespace FabricPlatform.API.Batches.Interfaces.Rest.Resources;

public record BatchResource(
    int Id,
    string BatchNumber,
    string GarmentModel,
    int ProjectedQuantity,
    int ProcessedQuantity,
    string CurrentStage,
    int ProgressPercentage,
    DateTimeOffset DeliveryDate,
    string Status,
    string? TechnicalSheetUrl,
    string Notes);