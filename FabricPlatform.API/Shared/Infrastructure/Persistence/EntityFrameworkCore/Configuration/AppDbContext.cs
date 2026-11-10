using FabricPlatform.API.Machinery.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Quality bounded context
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

        // Machinery bounded context
        builder.ApplyMachineryConfiguration();

        // Quality bounded context
        builder.ApplyConfiguration(new FabricInspectionConfiguration());
        builder.ApplyConfiguration(new DefectConfiguration());

        // Alerts bounded context
        builder.ApplyAlertConfiguration();

        builder.UseSnakeCaseNamingConvention();
    }
}
