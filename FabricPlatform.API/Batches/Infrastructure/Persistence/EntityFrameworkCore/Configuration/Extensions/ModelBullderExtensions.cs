using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyBatchesConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BatchConfiguration());
        modelBuilder.ApplyConfiguration(new BatchStageConfiguration());
        modelBuilder.ApplyConfiguration(new BatchOperatorConfiguration());
        modelBuilder.ApplyConfiguration(new BatchMovementConfiguration());
        modelBuilder.ApplyConfiguration(new BatchObservationConfiguration());
        
        return modelBuilder;
    }
}