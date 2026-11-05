using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Batches.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class BatchConfiguration : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Domain.Model.Aggregates.Batch>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Model.Aggregates.Batch> builder)
    {
        builder.ToTable("batches");

        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Id)
            .HasConversion(new ValueConverters.BatchIdValueConverter())
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Code)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasMany(b => b.Stages)
            .WithOne()
            .HasForeignKey(s => s.BatchId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        
        builder.Ignore(b => b.DomainEvents);
    }
}