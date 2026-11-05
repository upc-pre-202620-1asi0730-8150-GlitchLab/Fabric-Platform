using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class BatchOperatorConfiguration : IEntityTypeConfiguration<BatchOperator> 
{
    public void Configure(EntityTypeBuilder<BatchOperator> builder)
    {
        builder.ToTable("batch_operators");
        
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(o => o.BatchId)
            .HasConversion(new BatchIdValueConverter())
            .IsRequired();

        builder.Property(o => o.OperatorId)
            .IsRequired();

        builder.Property(o => o.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(o => o.Specialty)
            .HasMaxLength(100);

        builder.Property(o => o.AssignedStage)
            .IsRequired();

        builder.Property(o => o.AssignmentDate)
            .IsRequired();
    }
}