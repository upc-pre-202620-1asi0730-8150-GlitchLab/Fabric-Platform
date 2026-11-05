using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FabricPlatform.API.Batches.Domain.Model.Entities;
using FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class BatchObservationConfiguration : IEntityTypeConfiguration<BatchObservation>
{
    public void Configure(EntityTypeBuilder<BatchObservation> builder)
    {
        builder.ToTable("batch_observations");

        builder.HasKey(obs => obs.Id);

        builder.Property(obs => obs.Id)
            .ValueGeneratedOnAdd();

        builder.Property(obs => obs.BatchId)
            .HasConversion(new BatchIdValueConverter())
            .IsRequired();

        builder.Property(obs => obs.AuthorName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(obs => obs.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(obs => obs.Content)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(obs => obs.CreatedAt)
            .IsRequired();
    }
}