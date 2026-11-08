using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;
using FabricPlatform.API.Machinery.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Machinery.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MachineRepository(AppDbContext context) : BaseRepository<Machine>(context), IMachineRepository
{
    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await Context.Set<Machine>().AnyAsync(machine => machine.Code == code, cancellationToken);
    }

    public async Task<Machine?> FindByCodeAsNoTrackingAsync(string code, CancellationToken cancellationToken)
    {
        return await Context.Set<Machine>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(machine => machine.Downtimes)
            .FirstOrDefaultAsync(machine => machine.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<MachineDowntime>> FindDowntimesByMachineIdAsNoTrackingAsync(MachineId machineId,
        CancellationToken cancellationToken)
    {
        return await Context.Set<MachineDowntime>()
            .AsNoTracking()
            .Where(downtime => downtime.MachineId == machineId)
            .OrderBy(downtime => downtime.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MaintenanceRecord>> FindMaintenancesByMachineIdAsNoTrackingAsync(
        MachineId machineId, CancellationToken cancellationToken)
    {
        return await Context.Set<MaintenanceRecord>()
            .AsNoTracking()
            .Where(maintenance => maintenance.MachineId == machineId)
            .OrderBy(maintenance => maintenance.PerformedDate)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Machine?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Machine>()
            .AsSplitQuery()
            .Include(machine => machine.Downtimes)
            .Include(machine => machine.Maintenances)
            .FirstOrDefaultAsync(machine => machine.Id == new MachineId(id), cancellationToken);
    }

    public override async Task<Machine?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Machine>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(machine => machine.Downtimes)
            .FirstOrDefaultAsync(machine => machine.Id == new MachineId(id), cancellationToken);
    }

    public override async Task<IEnumerable<Machine>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Machine>()
            .Include(machine => machine.Downtimes)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<Machine>> ListAsNoTrackingAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Machine>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(machine => machine.Downtimes)
            .ToListAsync(cancellationToken);
    }
}
