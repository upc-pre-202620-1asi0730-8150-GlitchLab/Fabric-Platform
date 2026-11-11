using FabricPlatform.API.Batches.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class ProductionOperatorConfiguration : IEntityTypeConfiguration<ProductionOperator>
{
    public void Configure(EntityTypeBuilder<ProductionOperator> builder)
    {
        builder.ToTable("operators");
        builder.HasKey(operatorEntity => operatorEntity.Id);

        builder.Property(operatorEntity => operatorEntity.Id)
            .ValueGeneratedOnAdd();

        builder.Property(operatorEntity => operatorEntity.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(operatorEntity => operatorEntity.Specialty)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(operatorEntity => operatorEntity.Name)
            .IsUnique();

        builder.HasData(
            new { Id = 1, Name = "Carlos Mendoza", Specialty = "Overlock" },
            new { Id = 2, Name = "Ana Torres", Specialty = "Straight Stitch" },
            new { Id = 3, Name = "Luis Ramos", Specialty = "Finishing" },
            new { Id = 4, Name = "Maria Lopez", Specialty = "Quality Control" });
    }
}
