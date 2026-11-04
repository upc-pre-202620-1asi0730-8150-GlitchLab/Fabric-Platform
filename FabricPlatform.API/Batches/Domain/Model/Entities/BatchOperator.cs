using FabricPlatform.API.Batches.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Batches.Domain.Model.Entities;

public class BatchOperator
{
    public int Id { get; private set; }
    public BatchId BatchId { get; private set; }
    public int OperatorId { get; private set; }
    public string FullName { get; private set; }
    public string Specialty { get; private set; }
    public StageName AssignedStage { get; private set; }
    public DateTimeOffset AssignmentDate { get; private set; }
    
    protected BatchOperator() { }
    
    public BatchOperator(BatchId batchId, int operatorId, string fullName, string specialty, StageName assignedStage, DateTimeOffset assignmentDate)
    {
        BatchId = batchId;
        OperatorId = operatorId;
        FullName = fullName;
        Specialty = specialty;
        AssignedStage = assignedStage;
        AssignmentDate = assignmentDate;
    }
}