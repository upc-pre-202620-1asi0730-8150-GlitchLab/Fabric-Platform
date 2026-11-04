using FabricPlatform.API.Alerts.Domain.Model.Aggregates;
using FabricPlatform.API.Alerts.Domain.Repositories;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace FabricPlatform.API.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRepository(AppDbContext context)
    : BaseRepository<Alert>(context), IAlertRepository
{
}