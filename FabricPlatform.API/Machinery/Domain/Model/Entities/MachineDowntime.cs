using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.Entities;

public partial class MachineDowntime
{
    public MachineDowntime()
    {
        Reason = string.Empty;
    }

    public MachineDowntime(MachineId machineId, string reason, DateTime startTime) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        MachineId = machineId;
        Reason = reason.Trim();
        StartTime = startTime;
    }

    public MachineDowntimeId Id { get; }
    public MachineId MachineId { get; private set; }
    public string Reason { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public int? DurationMinutes { get; private set; }
    public string? Solution { get; private set; }

    public bool IsOpen => EndTime is null;

    public void Close(DateTime endTime, string? solution)
    {
        if (!IsOpen)
            throw new InvalidOperationException("The downtime is already closed.");
        if (endTime < StartTime)
            throw new ArgumentException("The end time cannot be earlier than the start time.", nameof(endTime));

        EndTime = endTime;
        DurationMinutes = (int)Math.Round((endTime - StartTime).TotalMinutes);
        Solution = string.IsNullOrWhiteSpace(solution) ? null : solution.Trim();
    }
}
