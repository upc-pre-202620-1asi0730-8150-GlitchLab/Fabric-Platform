namespace FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;

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

        builder.ApplyConfiguration(new FabricInspectionConfiguration());
        builder.ApplyConfiguration(new DefectConfiguration());
        
        builder.UseSnakeCaseNamingConvention();
    }
}