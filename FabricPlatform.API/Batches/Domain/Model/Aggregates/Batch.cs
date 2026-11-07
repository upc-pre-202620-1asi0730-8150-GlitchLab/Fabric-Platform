using FabricPlatform.API.Batches.Domain.Model.Events;
using FabricPlatform.API.Batches.Domain.Model.Entities;
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
    
    private readonly List<BatchStage> _stages = new();
    public IReadOnlyCollection<BatchStage> Stages => _stages.AsReadOnly();
    
    private readonly List<BatchOperator> _operators = new();
    public IReadOnlyCollection<BatchOperator> Operators => _operators.AsReadOnly();
    
    private readonly List<BatchMovement> _movements = new();
    public IReadOnlyCollection<BatchMovement> Movements => _movements.AsReadOnly();
    
    private readonly List<BatchObservation> _observations = new();
    public IReadOnlyCollection<BatchObservation> Observations => _observations.AsReadOnly();
    
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
    
    public void AddStage(StageName name, int order, int targetQuantity)
    {
        _stages.Add(new BatchStage(Id, name, order, targetQuantity));
    }
    public void AssignOperator(int operatorId, string fullName, string speciality, StageName assignedStage, DateTimeOffset assignmentDate)
    {
        _operators.Add(new BatchOperator(Id, operatorId, fullName, speciality, assignedStage, assignmentDate));
    }
    public void AddMovement(StageName stageName, int qtyIn, int qtyOut, string responsiblePerson, DateTimeOffset timestamp)
    {
        _movements.Add(new BatchMovement(Id, stageName, qtyIn, qtyOut, responsiblePerson, timestamp));
    }
    public void AddObservation(string authorName, string category, string content, DateTimeOffset createdAt)
    {
        _observations.Add(new BatchObservation(Id, authorName, category, content, createdAt));
    }
    public void UpdateStage(StageName newStageName, int processedQuantity)
    {
        var stage = _stages.FirstOrDefault(s => s.Name.Value == newStageName.Value);
        
        if (stage == null)
        {
            throw new InvalidOperationException($"The stage '{newStageName.Value}' does not exist for this batch.");
        }
        
        stage.UpdateProgress(processedQuantity);
        
        AddDomainEvent(new BatchStageUpdatedEvent(Id, newStageName));
    }
}