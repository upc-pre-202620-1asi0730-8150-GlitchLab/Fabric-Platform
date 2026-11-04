namespace FabricPlatform.API.Quality.Domain.Model.Aggregates;

using System;
using System.Collections.Generic;
using FabricPlatform.API.Quality.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Model.Aggregates.Internal;
using FabricPlatform.API.Shared.Domain.Model.Entities;

public class FabricInspection : AggregateRoot, IAuditableEntity
{
    private readonly List<InspectionParameterRecord> _parameterHistory = [];

    public FabricInspectionId Id { get; private set; }
    public string RollId { get; private set; } = string.Empty;
    public string Supplier { get; private set; } = string.Empty;
    public string InspectionDate { get; private set; } = string.Empty;
    public string Sample { get; private set; } = string.Empty;
    public string InitialResult { get; private set; } = "Conforming";
    
    public InspectionParameters? Parameters { get; private set; }
    public IReadOnlyCollection<InspectionParameterRecord> ParameterHistory => _parameterHistory.AsReadOnly();

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    protected FabricInspection() { }

    public FabricInspection(string rollId, string supplier, string inspectionDate, string sample, string initialResult)
    {
        RollId = rollId;
        Supplier = supplier;
        InspectionDate = inspectionDate;
        Sample = sample;
        InitialResult = initialResult;
    }

    public void UpdateParameters(
        int fabricWidth,
        int fabricWeight,
        string shadeCondition,
        string weaveCondition,
        string surfaceCondition,
        string defectsDetected,
        string? observations,
        IEnumerable<InspectionParameterRecord>? history = null)
    {
        Parameters = new InspectionParameters(
            fabricWidth,
            fabricWeight,
            shadeCondition,
            weaveCondition,
            surfaceCondition,
            defectsDetected,
            observations,
            DateTime.UtcNow);

        if (history != null)
        {
            _parameterHistory.Clear();
            _parameterHistory.AddRange(history);
        }
    }
}