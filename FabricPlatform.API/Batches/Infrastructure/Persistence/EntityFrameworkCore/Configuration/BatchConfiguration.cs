using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using FabricPlatform.API.Batches.Domain.Model.ValueObjects;
using FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("batches");

        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Id)
            .HasConversion(new BatchIdValueConverter())
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Code)
            .HasConversion(
                batchCode => batchCode.Value,
                value => new BatchCode(value)
            )
            .HasColumnName("code")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(b => b.Stages)
            .WithOne()
            .HasForeignKey(s => s.BatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(b => b.DomainEvents);
    }
}