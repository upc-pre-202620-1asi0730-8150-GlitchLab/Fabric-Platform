using FabricPlatform.API.Shared.Domain.Model.Errors;

namespace FabricPlatform.API.Machinery.Domain.Model.Errors;

public static class MachineryErrors
{
    public static readonly Error MachineNotFound =
        Error.NotFound("Machinery.MachineNotFound", "The specified machine was not found.");

    public static Error DuplicateMachineCodeWithCode(string code) =>
        Error.Conflict("Machinery.DuplicateMachineCode", $"A machine with the code '{code}' already exists.");

    public static readonly Error InvalidMachineData =
        Error.Validation("Machinery.InvalidMachineData",
            "The machine code, type, location and acquired date are required.");

    public static readonly Error InvalidMachineStatus =
        Error.Validation("Machinery.InvalidMachineStatus",
            "The machine status must be Operational, InMaintenance or Stopped.");

    public static readonly Error MachineAlreadyStopped =
        Error.Conflict("Machinery.MachineAlreadyStopped", "The machine already has an open downtime.");

    public static readonly Error DowntimeNotFound =
        Error.NotFound("Machinery.DowntimeNotFound", "The specified downtime was not found for this machine.");

    public static readonly Error DowntimeAlreadyClosed =
        Error.Conflict("Machinery.DowntimeAlreadyClosed", "The specified downtime is already closed.");

    public static readonly Error InvalidDowntimeData =
        Error.Validation("Machinery.InvalidDowntimeData", "The downtime reason and time are required.");

    public static readonly Error InvalidDowntimePeriod =
        Error.Validation("Machinery.InvalidDowntimePeriod", "The end time cannot be earlier than the start time.");

    public static readonly Error InvalidMaintenanceData =
        Error.Validation("Machinery.InvalidMaintenanceData",
            "The maintenance technician and performed date are required.");

    public static readonly Error InvalidMaintenanceType =
        Error.Validation("Machinery.InvalidMaintenanceType",
            "The maintenance type must be Preventive, Corrective or Calibration.");
}
