using FabricPlatform.API.Batches.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Batches.Domain.Model.Entities;

public class BatchMovement
{
    public int Id { get; private set; }
    public BatchId BatchId { get; private set; }
    public StageName StageName { get; private set; }
    public int QtyIn { get; private set; }
    public int QtyOut { get; private set; }
    public int Difference { get; private set; }
    public string ResponsiblePerson { get; private set; } = string.Empty;
    public DateTimeOffset Timestamp { get; private set; }
    public string Status { get; private set; } = "Current";

    protected BatchMovement()
    {
    }

    public BatchMovement(BatchId batchId, StageName stageName, int qtyIn, int qtyOut, int difference,
        string responsiblePerson, DateTimeOffset timestamp, string status = "Current")
    {
        BatchId = batchId;
        StageName = stageName;
        QtyIn = qtyIn;
        QtyOut = qtyOut;
        Difference = difference;
        ResponsiblePerson = string.IsNullOrWhiteSpace(responsiblePerson) ? "System" : responsiblePerson.Trim();
        Timestamp = timestamp;
        Status = string.IsNullOrWhiteSpace(status) ? "Current" : status;
    }

    public void Update(StageName? stageName, int? qtyIn, int? qtyOut, int? difference, string? responsible,
        string? status, DateTimeOffset? timestamp = null)
    {
        if (qtyIn.HasValue && qtyIn.Value < 0 || qtyOut.HasValue && qtyOut.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(qtyIn), "Movement quantities cannot be negative.");
        }

        if (stageName.HasValue)
        {
            StageName = stageName.Value;
        }

        if (qtyIn.HasValue)
        {
            QtyIn = qtyIn.Value;
        }

        if (qtyOut.HasValue)
        {
            QtyOut = qtyOut.Value;
        }

        if (difference.HasValue)
        {
            Difference = difference.Value;
        }

        ResponsiblePerson = string.IsNullOrWhiteSpace(responsible) ? ResponsiblePerson : responsible.Trim();
        Status = string.IsNullOrWhiteSpace(status) ? Status : status;
        if (timestamp.HasValue)
        {
            Timestamp = timestamp.Value;
        }
    }

    public void Complete()
    {
        Status = "Completed";
    }
}