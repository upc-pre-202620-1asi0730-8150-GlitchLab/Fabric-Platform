using FabricPlatform.API.Alerts.Domain.Model.Aggregates;
using FabricPlatform.API.Alerts.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRepository(AppDbContext context)
    : BaseRepository<Alert>(context), IAlertRepository
{
    public async Task<Alert?> FindByTypeAndMachineAsync(string type, string machineId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Alert>()
            .FirstOrDefaultAsync(alert => alert.Type == type && alert.Machine == machineId, cancellationToken);
    }
}