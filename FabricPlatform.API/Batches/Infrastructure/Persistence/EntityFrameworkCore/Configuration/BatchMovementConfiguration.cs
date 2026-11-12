using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class BatchMovementConfiguration : IEntityTypeConfiguration<BatchMovement>
{
    public void Configure(EntityTypeBuilder<BatchMovement> builder)
    {
        builder.ToTable("batch_movements");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.BatchId)
            .HasConversion(new BatchIdValueConverter())
            .IsRequired();

        builder.Property(m => m.StageName)
            .HasConversion(
                stageName => stageName.Value,
                value => new StageName(value)
            )
            .HasColumnName("stage_name")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.QtyIn)
            .IsRequired();

        builder.Property(m => m.QtyOut)
            .IsRequired();

        builder.Property(m => m.Difference)
            .HasColumnName("difference")
            .IsRequired();

        builder.Property(m => m.ResponsiblePerson)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(m => m.Timestamp)
            .IsRequired();

        builder.Property(m => m.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasMaxLength(50);
    }
}