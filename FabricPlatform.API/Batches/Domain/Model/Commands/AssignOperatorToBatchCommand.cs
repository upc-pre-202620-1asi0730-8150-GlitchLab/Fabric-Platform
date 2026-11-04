namespace FabricPlatform.API.Batches.Domain.Model.Commands;

public record AssignOperatorToBatchCommand
{
    int BatchId;
    int OperatorId;
    string FullName;
    string Specialty;
    string Stage;
}