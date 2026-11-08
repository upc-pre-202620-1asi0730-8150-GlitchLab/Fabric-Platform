using FabricPlatform.API.Machinery.Application.QueryServices;
using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Machinery.Domain.Model.Queries;
using FabricPlatform.API.Machinery.Domain.Repositories;

namespace FabricPlatform.API.Machinery.Application.Internal.QueryServices;

public class MachineQueryService(IMachineRepository machineRepository) : IMachineQueryService
{
    public async Task<IEnumerable<Machine>> Handle(GetAllMachinesQuery query, CancellationToken cancellationToken)
    {
        return await machineRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    public async Task<Machine?> Handle(GetMachineByIdQuery query, CancellationToken cancellationToken)
    {
        return await machineRepository.FindByIdAsNoTrackingAsync(query.MachineId, cancellationToken);
    }

    public async Task<Machine?> Handle(GetMachineByCodeQuery query, CancellationToken cancellationToken)
    {
        return await machineRepository.FindByCodeAsNoTrackingAsync(query.Code.Trim(), cancellationToken);
    }

    public async Task<IEnumerable<MachineDowntime>> Handle(GetMachineDowntimesByMachineIdQuery query,
        CancellationToken cancellationToken)
    {
        return await machineRepository.FindDowntimesByMachineIdAsNoTrackingAsync(query.MachineId, cancellationToken);
    }

    public async Task<IEnumerable<MaintenanceRecord>> Handle(GetMaintenanceRecordsByMachineIdQuery query,
        CancellationToken cancellationToken)
    {
        return await machineRepository.FindMaintenancesByMachineIdAsNoTrackingAsync(query.MachineId,
            cancellationToken);
    }
}
