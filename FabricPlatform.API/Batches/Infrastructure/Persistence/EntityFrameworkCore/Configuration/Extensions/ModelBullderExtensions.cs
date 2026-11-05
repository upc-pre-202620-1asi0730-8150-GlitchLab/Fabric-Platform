namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static Microsoft.EntityFrameworkCore.ModelBuilder ApplyBatchesConfiguration(this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BatchConfiguration());
        //modelBuilder.ApplyConfiguration(new BatchStageConfiguration());
        //modelBuilder.ApplyConfiguration(new BatchOperatorConfiguration());
        //modelBuilder.ApplyConfiguration(new BatchMovementConfiguration());
        //modelBuilder.ApplyConfiguration(new BatchObservationConfiguration());
        
        return modelBuilder;
    }
}