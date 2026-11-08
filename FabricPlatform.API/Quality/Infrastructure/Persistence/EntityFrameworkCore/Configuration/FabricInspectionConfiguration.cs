namespace FabricPlatform.API.Quality.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

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
        builder.Property(x => x.InitialResult).HasMaxLength(50);

        builder.OwnsOne(x => x.Parameters, p =>
        {
            p.ToJson();
        });

        builder.OwnsMany(x => x.ParameterHistory, h =>
        {
            h.ToJson();
        });
    }
}