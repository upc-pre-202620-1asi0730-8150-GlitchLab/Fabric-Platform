using FabricPlatform.API.Shared.Domain.Model.Errors;

namespace FabricPlatform.API.Batches.Domain.Model.Errors;

public static class BatchErrors
{
    public static readonly Error NotFound = Error.NotFound("Batch.NotFound", "The specified batch was not found.");
    public static readonly Error CodeAlreadyExists = Error.Conflict("Batch.CodeAlreadyExists", "A batch with this code already exists.");
    public static readonly Error InvalidQuantity = new("Batch.InvalidQuantity", "Projected quantity must be greater than zero.");
    public static readonly Error InvalidRequest = Error.Validation("Batch.InvalidRequest", "The request data is invalid.");
    public static readonly Error MovementNotFound = Error.NotFound("Batch.MovementNotFound", "The specified traceability movement was not found.");
    public static readonly Error OperatorNotFound = Error.NotFound("Batch.OperatorNotFound", "The specified operator was not found.");
    public static readonly Error OperatorAlreadyExists = Error.Conflict("Batch.OperatorAlreadyExists", "An operator with this name already exists.");
}