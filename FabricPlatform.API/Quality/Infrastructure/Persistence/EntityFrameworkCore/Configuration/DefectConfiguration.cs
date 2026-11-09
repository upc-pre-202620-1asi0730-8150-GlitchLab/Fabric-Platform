namespace FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

using System;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DefectConfiguration : IEntityTypeConfiguration<Defect>
{
    public void Configure(EntityTypeBuilder<Defect> builder)
    {
        builder.ToTable("defects");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdConverter<DefectId, int>(id => id.Value, val => new DefectId(val)))
            .ValueGeneratedOnAdd();

        builder.Property(x => x.BatchId).IsRequired().HasMaxLength(50);
        builder.Property(x => x.DefectType).IsRequired().HasMaxLength(100);
        builder.Property(x => x.MachineId).HasMaxLength(50);
        builder.Property(x => x.Origin).HasMaxLength(50);
        builder.Property(x => x.Status).HasMaxLength(50);
        builder.Property(x => x.Disposition).HasMaxLength(50);
        builder.Property(x => x.CorrectionType).HasMaxLength(50);
        builder.Property(x => x.Date).HasMaxLength(35);

        builder.Property(x => x.CreatedAt)
            .HasConversion(
                v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                v => v.HasValue ? new DateTimeOffset(v.Value, TimeSpan.Zero) : null);

        builder.Property(x => x.UpdatedAt)
            .HasConversion(
                v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                v => v.HasValue ? new DateTimeOffset(v.Value, TimeSpan.Zero) : null);
    }
}