using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class BatchStageConfiguration : IEntityTypeConfiguration<BatchStage>
{
    public void Configure(EntityTypeBuilder<BatchStage> builder)
    {
        builder.ToTable("batch_stages");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.BatchId)
            .HasConversion(new BatchIdValueConverter())
            .IsRequired();

        builder.Property(s => s.Name)
            .HasConversion(
                stageName => stageName.Value,
                value => new StageName(value)
            )
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Order)
            .IsRequired();

        builder.Property(s => s.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.ProcessedQuantity)
            .IsRequired();

        builder.Property(s => s.TargetQuantity)
            .IsRequired();
    }
}