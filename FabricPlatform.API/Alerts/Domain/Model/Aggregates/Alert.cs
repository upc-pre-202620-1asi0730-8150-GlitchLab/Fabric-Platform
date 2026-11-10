using FabricPlatform.API.Alerts.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace FabricPlatform.API.Alerts.Domain.Model.Aggregates;

public class Alert : AggregateRoot
{
    private Alert()
    {
    }

    public Alert(
        string status,
        string type,
        string reference,
        string title,
        string description,
        string action,
        string accumulated,
        string limit,
        string excess,
        string machine,
        string batch)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(status);
        ArgumentException.ThrowIfNullOrWhiteSpace(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        Status = status;
        Type = type;
        Reference = reference;
        Title = title;
        Description = description;
        Action = action;
        Accumulated = accumulated;
        Limit = limit;
        Excess = excess;
        Machine = machine;
        Batch = batch;
    }

    public AlertId Id { get; private set; }

    public string Status { get; private set; } = string.Empty;

    public string Type { get; private set; } = string.Empty;

    public string Reference { get; private set; } = string.Empty;

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string Action { get; private set; } = string.Empty;

    public string Accumulated { get; private set; } = string.Empty;

    public string Limit { get; private set; } = string.Empty;

    public string Excess { get; private set; } = string.Empty;

    public string Machine { get; private set; } = string.Empty;

    public string Batch { get; private set; } = string.Empty;
}