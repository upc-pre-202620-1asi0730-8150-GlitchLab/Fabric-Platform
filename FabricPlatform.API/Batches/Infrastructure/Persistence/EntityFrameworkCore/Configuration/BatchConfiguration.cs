using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("batches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                batchId => batchId.Value,
                value => new BatchId(value))
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Code)
            .HasConversion(
                code => code.Value,
                value => new BatchCode(value))
            .HasColumnName("code")
            .HasMaxLength(50);

        builder.Property(b => b.GarmentModel)
            .HasColumnName("garment_model")
            .HasMaxLength(200);

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasColumnName("status");

        builder.Property(b => b.ProjectedQuantity)
            .HasColumnName("projected_quantity");

        builder.Property(b => b.ProcessedQuantity)
            .HasColumnName("processed_quantity")
            .HasDefaultValue(0);

        builder.Property(b => b.CurrentStage)
            .HasConversion(
                stage => stage.Value,
                value => new StageName(value))
            .HasColumnName("current_stage")
            .HasMaxLength(100);

        builder.Property(b => b.ProgressPercentage)
            .HasColumnName("progress_percentage");

        builder.Property(b => b.DeliveryDate)
            .HasColumnName("delivery_date");

        builder.Property(b => b.TechnicalSheetUrl)
            .HasColumnName("technical_sheet_url")
            .HasMaxLength(500);

        builder.Property(b => b.Notes)
            .HasColumnName("notes")
            .HasMaxLength(1000);

        builder.HasMany(b => b.Stages)
            .WithOne()
            .HasForeignKey("BatchId");

        builder.HasMany(b => b.Operators)
            .WithOne()
            .HasForeignKey("BatchId");

        builder.HasMany(b => b.Movements)
            .WithOne()
            .HasForeignKey("BatchId");

        builder.HasMany(b => b.Observations)
            .WithOne()
            .HasForeignKey("BatchId");
    }
}