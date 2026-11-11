using FabricPlatform.API.Machinery.Domain.Model.Aggregates;
using FabricPlatform.API.Machinery.Domain.Model.Entities;
using FabricPlatform.API.Machinery.Domain.Model.ValueObjects;
using FabricPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace FabricPlatform.API.Machinery.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyMachineryConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Machine>().HasKey(m => m.Id);
        builder.Entity<Machine>().Property(m => m.Id)
            .HasConversion(new StronglyTypedIdConverter<MachineId, int>(id => id.Value, value => new MachineId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Machine>().Property(m => m.Code).IsRequired().HasMaxLength(30);
        builder.Entity<Machine>().HasIndex(m => m.Code).IsUnique();
        builder.Entity<Machine>().Property(m => m.Type).IsRequired().HasMaxLength(50);
        builder.Entity<Machine>().Property(m => m.Location).IsRequired().HasMaxLength(100);
        builder.Entity<Machine>().Property(m => m.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<Machine>().Property(m => m.AcquiredDate).IsRequired();
        builder.Entity<Machine>().Ignore(m => m.AccumulatedDowntimeMinutes);
        builder.Entity<Machine>().Ignore(m => m.HasOpenDowntime);

        builder.Entity<MachineDowntime>().HasKey(d => d.Id);
        builder.Entity<MachineDowntime>().Property(d => d.Id)
            .HasConversion(new StronglyTypedIdConverter<MachineDowntimeId, int>(id => id.Value,
                value => new MachineDowntimeId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<MachineDowntime>().Property(d => d.MachineId)
            .HasConversion(new StronglyTypedIdConverter<MachineId, int>(id => id.Value, value => new MachineId(value)))
            .IsRequired();
        builder.Entity<MachineDowntime>().Property(d => d.Reason).IsRequired().HasMaxLength(120);
        builder.Entity<MachineDowntime>().Property(d => d.StartTime).IsRequired();
        builder.Entity<MachineDowntime>().Property(d => d.Solution).HasMaxLength(240);
        builder.Entity<MachineDowntime>().Ignore(d => d.IsOpen);

        builder.Entity<MaintenanceRecord>().HasKey(r => r.Id);
        builder.Entity<MaintenanceRecord>().Property(r => r.Id)
            .HasConversion(new StronglyTypedIdConverter<MaintenanceRecordId, int>(id => id.Value,
                value => new MaintenanceRecordId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<MaintenanceRecord>().Property(r => r.MachineId)
            .HasConversion(new StronglyTypedIdConverter<MachineId, int>(id => id.Value, value => new MachineId(value)))
            .IsRequired();
        builder.Entity<MaintenanceRecord>().Property(r => r.Technician).IsRequired().HasMaxLength(80);
        builder.Entity<MaintenanceRecord>().Property(r => r.MaintenanceType)
            .HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<MaintenanceRecord>().Property(r => r.Description).HasMaxLength(240);
        builder.Entity<MaintenanceRecord>().Property(r => r.PerformedDate).IsRequired();

        builder.Entity<Machine>()
            .HasMany(m => m.Downtimes)
            .WithOne()
            .HasForeignKey(d => d.MachineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Machine>()
            .HasMany(m => m.Maintenances)
            .WithOne()
            .HasForeignKey(r => r.MachineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
