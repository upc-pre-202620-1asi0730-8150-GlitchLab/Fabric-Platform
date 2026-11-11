using FabricPlatform.API.Machinery.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;
using FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<FabricInspection> FabricInspections => Set<FabricInspection>();
    public DbSet<Defect> Defects => Set<Defect>();

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());

        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyMachineryConfiguration();

        builder.ApplyConfiguration(new FabricInspectionConfiguration());
        builder.ApplyConfiguration(new DefectConfiguration());

        builder.ApplyAlertConfiguration();

        builder.UseSnakeCaseNamingConvention();
        builder.ApplyConfigurationsFromAssembly(typeof(BatchConfiguration).Assembly);
    }
}
