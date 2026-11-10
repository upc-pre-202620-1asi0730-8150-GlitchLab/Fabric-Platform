namespace FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

public record MachineResource(
    int Id,
    string Code,
    string Type,
    string Status,
    string Location,
    DateTime AcquiredDate,
    int AccumulatedDowntimeMinutes);
