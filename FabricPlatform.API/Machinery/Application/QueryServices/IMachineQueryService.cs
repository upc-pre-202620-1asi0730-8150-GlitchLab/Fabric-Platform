using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Machinery.Domain.Model.Queries;

namespace FabricPlatform.API.Machinery.Application.QueryServices;

public interface IMachineQueryService
{
    Task<IEnumerable<Machine>> Handle(GetAllMachinesQuery query, CancellationToken cancellationToken);

    Task<Machine?> Handle(GetMachineByIdQuery query, CancellationToken cancellationToken);

    Task<Machine?> Handle(GetMachineByCodeQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<MachineDowntime>> Handle(GetMachineDowntimesByMachineIdQuery query,
        CancellationToken cancellationToken);

    Task<IEnumerable<MaintenanceRecord>> Handle(GetMaintenanceRecordsByMachineIdQuery query,
        CancellationToken cancellationToken);
}
