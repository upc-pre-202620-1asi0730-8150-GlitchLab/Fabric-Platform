using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Domain.Model.Events;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace FabricPlatform.API.Batches.Domain.Model.Aggregates;

public class Batch : AggregateRoot
{
    public BatchId Id { get; private set; }
    public BatchCode Code { get; private set; }
    public string GarmentModel { get; private set; } = string.Empty;
    public BatchStatus Status { get; private set; }
    public int ProjectedQuantity { get; private set; }
    public int ProcessedQuantity { get; private set; }
    public StageName CurrentStage { get; private set; }
    public int ProgressPercentage { get; private set; }
    public DateTimeOffset DeliveryDate { get; private set; }
    public string? TechnicalSheetUrl { get; private set; }
    public string Notes { get; private set; } = string.Empty;

    private readonly List<BatchStage> _stages = new();
    public IReadOnlyCollection<BatchStage> Stages => _stages.AsReadOnly();

    private readonly List<BatchOperator> _operators = new();
    public IReadOnlyCollection<BatchOperator> Operators => _operators.AsReadOnly();

    private readonly List<BatchMovement> _movements = new();
    public IReadOnlyCollection<BatchMovement> Movements => _movements.AsReadOnly();

    private readonly List<BatchObservation> _observations = new();
    public IReadOnlyCollection<BatchObservation> Observations => _observations.AsReadOnly();

    protected Batch()
    {
    }

    public Batch(BatchCode code, string garmentModel, int projectedQuantity, string currentStage,
        int progressPercentage, DateTimeOffset deliveryDate, BatchStatus status = BatchStatus.InProduction,
        string? technicalSheetUrl = null, string? notes = null)
    {
        Code = code;
        GarmentModel = !string.IsNullOrWhiteSpace(garmentModel)
            ? garmentModel.Trim()
            : throw new ArgumentException("Garment model is required.", nameof(garmentModel));
        ProjectedQuantity = projectedQuantity > 0
            ? projectedQuantity
            : throw new ArgumentException("Projected quantity must be greater than zero.", nameof(projectedQuantity));
        CurrentStage = new StageName(currentStage);
        ProgressPercentage = Math.Clamp(progressPercentage, 0, 100);
        DeliveryDate = deliveryDate;
        Status = status;
        TechnicalSheetUrl = technicalSheetUrl;
        Notes = notes ?? string.Empty;

        _stages.Add(new BatchStage(Id, new StageName("Cutting"), 1, ProjectedQuantity));
        _stages.Add(new BatchStage(Id, new StageName("Sewing"), 2, ProjectedQuantity));
        _stages.Add(new BatchStage(Id, new StageName("Finishing"), 3, ProjectedQuantity));
        _stages.Add(new BatchStage(Id, new StageName("Quality Check"), 4, ProjectedQuantity));
        _stages.Add(new BatchStage(Id, new StageName("Completed"), 5, ProjectedQuantity));

        AddDomainEvent(new BatchCreatedEvent(this));
    }

    public void AssignGeneratedCode()
    {
        if (Id.Value <= 0)
        {
            throw new InvalidOperationException(
                "A batch code can only be generated after the batch has been persisted.");
        }

        Code = new BatchCode($"LOT-{Id.Value:000}");
    }

    public void UpdateProductionDetails(
        StageName stage,
        int progressPercentage,
        int? processedQuantity,
        string? notes,
        DateTimeOffset updatedAt)
    {
        if (processedQuantity.HasValue)
        {
            ProcessedQuantity = processedQuantity.Value;
            if (processedQuantity.Value > 0)
            {
                foreach (var movement in _movements.Where(movement =>
                             movement.Status.Equals("Current", StringComparison.OrdinalIgnoreCase)))
                {
                    movement.Complete();
                }

                var previousMovement = _movements
                    .OrderBy(movement => movement.Timestamp)
                    .ThenBy(movement => movement.Id)
                    .LastOrDefault();
                var qtyIn = previousMovement?.QtyOut ?? ProjectedQuantity;
                AddMovement(
                    CurrentStage,
                    qtyIn,
                    processedQuantity.Value,
                    processedQuantity.Value - qtyIn,
                    "Supervisor",
                    updatedAt);
            }
        }

        if (notes != null)
        {
            Notes = notes;
        }

        UpdateStage(stage, progressPercentage);
    }

    public BatchTraceabilitySummary GetTraceabilitySummary()
    {
        var currentQuantity = _movements
            .OrderBy(movement => movement.Timestamp)
            .ThenBy(movement => movement.Id)
            .LastOrDefault()?.QtyOut ?? ProjectedQuantity;

        return new BatchTraceabilitySummary(
            Id.Value,
            ProjectedQuantity,
            currentQuantity,
            currentQuantity - ProjectedQuantity,
            CurrentStage);
    }

    public void ApplyPatch(string? garmentModel, int? projectedQuantity, string? currentStage, int? progressPercentage,
        DateTimeOffset? deliveryDate, string? status, string? technicalSheetUrl, string? notes)
    {
        if (!string.IsNullOrWhiteSpace(garmentModel))
        {
            GarmentModel = garmentModel.Trim();
        }

        if (projectedQuantity.HasValue)
        {
            if (projectedQuantity.Value <= 0)
            {
                throw new ArgumentException("Projected quantity must be greater than zero.", nameof(projectedQuantity));
            }

            ProjectedQuantity = projectedQuantity.Value;
        }

        if (deliveryDate.HasValue)
        {
            DeliveryDate = deliveryDate.Value;
        }

        if (!string.IsNullOrWhiteSpace(technicalSheetUrl))
        {
            TechnicalSheetUrl = technicalSheetUrl;
        }

        if (notes != null)
        {
            Notes = notes;
        }

        if (!string.IsNullOrWhiteSpace(currentStage))
        {
            UpdateStage(new StageName(currentStage), progressPercentage ?? ProgressPercentage);
        }
        else if (progressPercentage.HasValue)
        {
            ProgressPercentage = Math.Clamp(progressPercentage.Value, 0, 100);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Status = ParseStatus(status);
        }

        if (CurrentStage.Value.Equals("Completed", StringComparison.OrdinalIgnoreCase) || ProgressPercentage >= 100)
        {
            Status = BatchStatus.Completed;
        }
    }

    public void AddStage(StageName name, int order, int targetQuantity)
    {
        _stages.Add(new BatchStage(Id, name, order, targetQuantity));
    }

    public BatchOperator AssignOperator(int operatorId, string fullName, string speciality, StageName assignedStage,
        DateTimeOffset assignmentDate)
    {
        var assignedOperator = new BatchOperator(Id, operatorId, fullName, speciality, assignedStage, assignmentDate);
        _operators.Add(assignedOperator);
        return assignedOperator;
    }

    public BatchMovement AddMovement(StageName stageName, int qtyIn, int qtyOut, int difference,
        string responsiblePerson, DateTimeOffset timestamp, string status = "Current")
    {
        if (qtyIn < 0 || qtyOut < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(qtyIn), "Movement quantities cannot be negative.");
        }

        var currentStatus = string.IsNullOrWhiteSpace(status) ? "Current" : status;
        var movement = new BatchMovement(Id, stageName, qtyIn, qtyOut, difference, responsiblePerson, timestamp,
            currentStatus);
        _movements.Add(movement);
        return movement;
    }

    public bool UpdateMovement(
        int movementId,
        StageName? stageName,
        int? qtyIn,
        int? qtyOut,
        int? difference,
        string? responsiblePerson,
        DateTimeOffset? timestamp,
        string? status)
    {
        var movement = _movements.FirstOrDefault(item => item.Id == movementId);
        if (movement == null)
        {
            return false;
        }

        movement.Update(stageName, qtyIn, qtyOut, difference, responsiblePerson, status, timestamp);
        return true;
    }

    public BatchObservation AddObservation(string authorName, string category, string content, DateTimeOffset createdAt)
    {
        if (string.IsNullOrWhiteSpace(authorName) || string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Observation author and text are required.");
        }

        var observation = new BatchObservation(Id, authorName, category, content, createdAt);
        _observations.Add(observation);
        return observation;
    }

    public void UpdateStage(StageName newStageName, int progressPercentage)
    {
        CurrentStage = newStageName;
        ProgressPercentage = Math.Clamp(progressPercentage, 0, 100);

        if (newStageName.Value.Equals("Completed", StringComparison.OrdinalIgnoreCase) || ProgressPercentage >= 100)
        {
            Status = BatchStatus.Completed;
            CurrentStage = new StageName("Completed");
            ProgressPercentage = 100;
        }
        else if (Status == BatchStatus.Completed)
        {
            Status = BatchStatus.InProduction;
        }

        var stage = _stages.FirstOrDefault(s =>
            s.Name.Value.Equals(newStageName.Value, StringComparison.OrdinalIgnoreCase));
        if (stage != null)
        {
            stage.UpdateProgress(ProgressPercentage);
        }

        AddDomainEvent(new BatchStageUpdatedEvent(Id, newStageName));
    }

    public void UpdateStatus(BatchStatus status)
    {
        Status = status;
    }

    private static BatchStatus ParseStatus(string status)
    {
        return BatchStatusExtensions.TryParse(status, out var parsed)
            ? parsed
            : throw new ArgumentException($"The status '{status}' is not valid for a batch.", nameof(status));
    }
}