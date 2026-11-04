using FabricPlatform.API.Batches.Domain.Model.Events;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace FabricPlatform.API.Batches.Domain.Model.Aggregates;

public class Batch : AggregateRoot
{
    public BatchId Id { get; private set; }
    public BatchCode Code { get; private set; }
    public BatchStatus Status { get; private set; }
    public int ProjectedQuantity { get; private set; }
    public DateTimeOffset DeliveryDate { get; private set; }
    
    protected Batch() { }
    
    public Batch( BatchCode code, int projectedQuantity, DateTimeOffset deliveryDate)
    {
        Code = code;
        ProjectedQuantity = projectedQuantity > 0
            ? projectedQuantity
            : throw new ArgumentException("Projected quantity must be greater than zero.", nameof(projectedQuantity));
        DeliveryDate = deliveryDate;
        Status = BatchStatus.InProduction;
        
        AddDomainEvent(new BatchCreatedEvent(this));
    }
}