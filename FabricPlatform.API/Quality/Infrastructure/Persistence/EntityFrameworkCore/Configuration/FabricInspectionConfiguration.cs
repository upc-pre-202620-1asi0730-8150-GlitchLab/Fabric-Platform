namespace FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

using System;
using System.Collections.Generic;
using System.Text.Json;
using FabricPlatform.API.Quality.Domain.Model.Aggregates;
using FabricPlatform.API.Quality.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FabricInspectionConfiguration : IEntityTypeConfiguration<FabricInspection>
{
    public void Configure(EntityTypeBuilder<FabricInspection> builder)
    {
        builder.ToTable("fabric_inspections");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(new StronglyTypedIdConverter<FabricInspectionId, int>(id => id.Value, val => new FabricInspectionId(val)))
            .ValueGeneratedOnAdd();

        builder.Property(x => x.RollId).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Supplier).IsRequired().HasMaxLength(150);
        builder.Property(x => x.InspectionDate).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Sample).HasMaxLength(100);
        builder.Property(x => x.Color).HasMaxLength(50);
        builder.Property(x => x.RollLength).HasPrecision(10, 2);
        builder.Property(x => x.ShadeMatch).HasMaxLength(50);
        builder.Property(x => x.WeaveCondition).HasMaxLength(50);
        builder.Property(x => x.Observation).HasMaxLength(500);
        builder.Property(x => x.InitialResult).HasMaxLength(50);
        builder.Property(x => x.RollStatus).HasMaxLength(50);
        builder.Property(x => x.ShrinkageTest).HasMaxLength(50);

        builder.Property(x => x.CreatedAt)
            .HasConversion(
                v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                v => v.HasValue ? new DateTimeOffset(v.Value, TimeSpan.Zero) : null);

        builder.Property(x => x.UpdatedAt)
            .HasConversion(
                v => v.HasValue ? v.Value.UtcDateTime : (DateTime?)null,
                v => v.HasValue ? new DateTimeOffset(v.Value, TimeSpan.Zero) : null);

        builder.Property(x => x.Parameters)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<InspectionParameters>(v, (JsonSerializerOptions?)null));

        builder.Property(x => x.ParameterHistory)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => string.IsNullOrEmpty(v) ? new List<InspectionParameterRecord>() : JsonSerializer.Deserialize<List<InspectionParameterRecord>>(v, (JsonSerializerOptions?)null) ?? new List<InspectionParameterRecord>())
            .HasField("_parameterHistory");

        builder.Property(x => x.TestHistory)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => string.IsNullOrEmpty(v) ? new List<ShrinkageTestRecord>() : JsonSerializer.Deserialize<List<ShrinkageTestRecord>>(v, (JsonSerializerOptions?)null) ?? new List<ShrinkageTestRecord>())
            .HasField("_testHistory");
    }
}