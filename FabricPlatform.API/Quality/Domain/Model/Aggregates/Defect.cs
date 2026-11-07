namespace FabricPlatform.API.Quality.Domain.Model.Aggregates;

using System;
using FabricPlatform.API.Quality.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Model.Aggregates.Internal;
using FabricPlatform.API.Shared.Domain.Model.Entities;

public class Defect : AggregateRoot, IAuditableEntity
{
    public DefectId Id { get; private set; }
    public string BatchId { get; private set; } = string.Empty;
    public string DefectType { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public string? MachineId { get; private set; }
    public string Origin { get; private set; } = "Known";
    public string Status { get; private set; } = "Pending Decision";
    public string Observation { get; private set; } = string.Empty;
    public string? Disposition { get; private set; }
    public string? CorrectionType { get; private set; }
    public string Date { get; private set; } = DateTime.UtcNow.ToString("o");

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    protected Defect() { }

    public Defect(
        string batchId,
        string defectType,
        int quantity,
        string? machineId = null,
        string origin = "Known",
        string status = "Pending Decision",
        string observation = "",
        string? disposition = null,
        string? correctionType = null,
        string? date = null)
    {
        BatchId = batchId;
        DefectType = defectType;
        Quantity = quantity;
        MachineId = machineId;
        Origin = origin;
        Status = status;
        Observation = observation;
        Disposition = disposition;
        CorrectionType = correctionType;
        Date = date ?? DateTime.UtcNow.ToString("o");
    }

    public void Update(
        string? status,
        string? observation,
        string? disposition,
        string? correctionType)
    {
        if (!string.IsNullOrWhiteSpace(status)) Status = status;
        if (observation != null) Observation = observation;
        if (disposition != null) Disposition = disposition;
        if (correctionType != null) CorrectionType = correctionType;
    }
}