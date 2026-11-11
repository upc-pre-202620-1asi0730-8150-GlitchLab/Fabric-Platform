namespace FabricPlatform.API.Alerts.Interfaces.Rest.Resources;

public record AlertResource(
    string Id,
    string Status,
    string Type,
    string Reference,
    string Title,
    string Description,
    string Action,
    string Accumulated,
    string Limit,
    string Excess,
    string Machine,
    string Batch);