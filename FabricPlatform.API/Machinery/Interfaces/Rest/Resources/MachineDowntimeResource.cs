namespace FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

public record MachineDowntimeResource(
    int Id,
    int MachineId,
    string Reason,
    DateTime StartTime,
    DateTime? EndTime,
    int? DurationMinutes,
    string? Solution);
