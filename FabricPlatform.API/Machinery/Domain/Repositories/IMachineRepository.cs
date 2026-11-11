using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Domain.Repositories;

namespace FabricPlatform.API.Machinery.Domain.Repositories;

public interface IMachineRepository : IBaseRepository<Machine>
{
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken);

    Task<Machine?> FindByCodeAsNoTrackingAsync(string code, CancellationToken cancellationToken);

    Task<IEnumerable<MachineDowntime>> FindDowntimesByMachineIdAsNoTrackingAsync(MachineId machineId,
        CancellationToken cancellationToken);

    Task<IEnumerable<MaintenanceRecord>> FindMaintenancesByMachineIdAsNoTrackingAsync(MachineId machineId,
        CancellationToken cancellationToken);
}
