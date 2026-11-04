using FabricPlatform.API.Batches.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Batches.Domain.Model.Entities;

public class BatchStage
{
    public int Id { get; private set; }
    public BatchId BatchId { get; private set; }
    public StageName Name { get; private set; }
    public int Order { get; private set; }
    public string Status { get; private set; }
    public int ProcessedQuantity { get; private set; }
    public int TargetQuantity { get; private set; }
    
    protected BatchStage() { }

    public BatchStage(BatchId batchId, StageName name, int order, int targetQuantity)
    {
        BatchId = batchId;
        Name = name;
        Order = order;
        TargetQuantity = targetQuantity;
        Status = "Pending";
        ProcessedQuantity = 0;
    }
    public void UpdateProgress(int processedQuantity)
    {
        ProcessedQuantity = processedQuantity;
        if (ProcessedQuantity >= TargetQuantity)
        {
            Status = "Completed";
        }
        else if (ProcessedQuantity > 0)
        {
            Status = "In Progress";
        }
    }
}