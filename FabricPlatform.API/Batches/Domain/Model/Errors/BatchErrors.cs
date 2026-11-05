using FabricPlatform.API.Shared.Domain.Model.Errors;

namespace FabricPlatform.API.Batches.Domain.Model.Errors;

public static class BatchErrors
{
    public static readonly Error NotFound = new("Batch.NotFound", "The specified batch was not found.");
    public static readonly Error CodeAlreadyExists = new("Batch.CodeAlreadyExists", "A batch with this code already exists.");
    public static readonly Error InvalidQuantity = new("Batch.InvalidQuantity", "Projected quantity must be greater than zero.");
}