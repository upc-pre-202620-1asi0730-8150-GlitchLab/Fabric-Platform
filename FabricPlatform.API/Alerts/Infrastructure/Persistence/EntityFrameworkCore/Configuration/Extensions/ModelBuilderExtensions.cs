using FabricPlatform.API.Alerts.Domain.Model.Aggregates;
using FabricPlatform.API.Alerts.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAlertsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Alert>().HasKey(alert => alert.Id);

        builder.Entity<Alert>()
            .Property(alert => alert.Id)
            .HasConversion(
                new StronglyTypedIdConverter<AlertId, int>(
                    id => id.Value,
                    value => new AlertId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<Alert>()
            .Property(alert => alert.Status)
            .IsRequired()
            .HasMaxLength(30);

        builder.Entity<Alert>()
            .Property(alert => alert.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Entity<Alert>()
            .Property(alert => alert.Reference)
            .HasMaxLength(50);

        builder.Entity<Alert>()
            .Property(alert => alert.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Entity<Alert>()
            .Property(alert => alert.Description)
            .HasMaxLength(500);

        builder.Entity<Alert>()
            .Property(alert => alert.Action)
            .HasMaxLength(100);

        builder.Entity<Alert>()
            .Property(alert => alert.Accumulated)
            .HasMaxLength(50);

        builder.Entity<Alert>()
            .Property(alert => alert.Limit)
            .HasMaxLength(50);

        builder.Entity<Alert>()
            .Property(alert => alert.Excess)
            .HasMaxLength(50);

        builder.Entity<Alert>()
            .Property(alert => alert.Machine)
            .HasMaxLength(50);

        builder.Entity<Alert>()
            .Property(alert => alert.Batch)
            .HasMaxLength(50);
    }
}