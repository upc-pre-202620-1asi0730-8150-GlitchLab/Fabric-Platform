namespace FabricPlatform.API.Quality.Domain.Model.Aggregates;

using System;
using System.Collections.Generic;
using FabricPlatform.API.Quality.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Model.Aggregates.Internal;
using FabricPlatform.API.Shared.Domain.Model.Entities;

public class FabricInspection : AggregateRoot, IAuditableEntity
{
    private readonly List<InspectionParameterRecord> _parameterHistory = [];
    private readonly List<ShrinkageTestRecord> _testHistory = [];

    public static readonly IReadOnlyCollection<string> RollStatuses = ["Available for Cutting", "Blocked"];

    public FabricInspectionId Id { get; private set; }
    public string RollId { get; private set; } = string.Empty;
    public string Supplier { get; private set; } = string.Empty;
    public string InspectionDate { get; private set; } = string.Empty;
    public string Sample { get; private set; } = string.Empty;
    public string Color { get; private set; } = "Navy";
    public decimal? RollLength { get; private set; }
    public string ShadeMatch { get; private set; } = string.Empty;
    public string WeaveCondition { get; private set; } = string.Empty;
    public string Observation { get; private set; } = string.Empty;
    public string InitialResult { get; private set; } = "Conforming";
    public string RollStatus { get; private set; } = "Available for Cutting";
    public string ShrinkageTest { get; private set; } = "Pending";

    public InspectionParameters? Parameters { get; private set; }
    public IReadOnlyCollection<InspectionParameterRecord> ParameterHistory => _parameterHistory.AsReadOnly();
    public IReadOnlyCollection<ShrinkageTestRecord> TestHistory => _testHistory.AsReadOnly();

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    protected FabricInspection() { }

    public FabricInspection(
        string rollId,
        string supplier,
        string inspectionDate,
        string sample,
        string color = "Navy",
        string initialResult = "Conforming",
        string rollStatus = "Available for Cutting",
        decimal? rollLength = null,
        string shadeMatch = "",
        string weaveCondition = "",
        string observation = "")
    {
        RollId = rollId;
        Supplier = supplier;
        InspectionDate = inspectionDate;
        Sample = sample;
        Color = color;
        InitialResult = initialResult;
        RollStatus = rollStatus;
        RollLength = rollLength;
        ShadeMatch = shadeMatch;
        WeaveCondition = weaveCondition;
        Observation = observation;
        ShrinkageTest = "Pending";
    }

    public void UpdateParameters(
        decimal fabricWidth,
        int? fabricWeight,
        decimal? fabricLength,
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
            fabricLength,
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

    public void UpdateRollStatus(string rollStatus)
    {
        RollStatus = rollStatus;
    }

    public void UpdateShrinkageTest(
        string shrinkageResult,
        IEnumerable<ShrinkageTestRecord>? tests = null)
    {
        ShrinkageTest = shrinkageResult;

        if (tests != null)
        {
            _testHistory.Clear();
            _testHistory.AddRange(tests);
        }

        if (shrinkageResult.Equals("Non-Compliant", StringComparison.OrdinalIgnoreCase))
        {
            RollStatus = "Blocked";
        }
    }
}