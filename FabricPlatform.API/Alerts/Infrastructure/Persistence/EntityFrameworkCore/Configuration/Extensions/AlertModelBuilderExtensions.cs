using FabricPlatform.API.Alerts.Domain.Model.Aggregates;
using FabricPlatform.API.Alerts.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class AlertModelBuilderExtensions
{
    public static void ApplyAlertConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Alert>(entity =>
        {
            entity.HasKey(alert => alert.Id);

            entity.Property(alert => alert.Id)
                .HasConversion(
                    new StronglyTypedIdConverter<AlertId, int>(
                        id => id.Value,
                        value => new AlertId(value)))
                .ValueGeneratedOnAdd();

            entity.Property(alert => alert.Status)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(alert => alert.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(alert => alert.Reference)
                .HasMaxLength(50);

            entity.Property(alert => alert.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(alert => alert.Description)
                .HasMaxLength(500);

            entity.Property(alert => alert.Action)
                .HasMaxLength(100);

            entity.Property(alert => alert.Accumulated)
                .HasMaxLength(50);

            entity.Property(alert => alert.Limit)
                .HasMaxLength(50);

            entity.Property(alert => alert.Excess)
                .HasMaxLength(50);

            entity.Property(alert => alert.Machine)
                .HasMaxLength(50);

            entity.Property(alert => alert.Batch)
                .HasMaxLength(50);
        });
    }
}