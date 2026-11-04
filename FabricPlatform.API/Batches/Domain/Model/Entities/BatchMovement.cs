using FabricPlatform.API.Batches.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Batches.Domain.Model.Entities;

public class BatchMovement
{
    public int Id { get; private set; }
    public BatchId BatchId { get; private set; }
    public StageName StageName { get; private set; }
    public int QtyIn { get; private set; }
    public int QtyOut { get; private set; }
    public int Discrepancy => QtyIn - QtyOut;
    public string ResponsiblePerson { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    
    protected BatchMovement() { }
    
    public BatchMovement(BatchId batchId, StageName stageName, int qtyIn, int qtyOut, string responsiblePerson, DateTimeOffset timestamp)
    {
        BatchId = batchId;
        StageName = stageName;
        QtyIn = qtyIn;
        QtyOut = qtyOut;
        ResponsiblePerson = responsiblePerson;
        Timestamp = DateTimeOffset.UtcNow;
    }
}