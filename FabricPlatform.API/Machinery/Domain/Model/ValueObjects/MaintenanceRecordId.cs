using FabricPlatform.API.Shared.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

public readonly record struct MaintenanceRecordId(int Value) : IEntityId<int>
{
    public static implicit operator int(MaintenanceRecordId id) => id.Value;
    public static implicit operator MaintenanceRecordId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
