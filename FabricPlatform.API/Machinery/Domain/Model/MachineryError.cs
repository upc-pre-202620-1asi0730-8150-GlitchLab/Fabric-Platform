namespace FabricPlatform.API.Machinery.Domain.Model;

public enum MachineryError
{
    None,
    MachineNotFound,
    DuplicateMachineCode,
    InvalidMachineData,
    InvalidMachineStatus,
    MachineAlreadyStopped,
    DowntimeNotFound,
    DowntimeAlreadyClosed,
    InvalidDowntimeData,
    InvalidDowntimePeriod,
    InvalidMaintenanceData,
    InvalidMaintenanceType,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
