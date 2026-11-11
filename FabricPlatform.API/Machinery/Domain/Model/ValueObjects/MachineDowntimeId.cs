using FabricPlatform.API.Shared.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

public readonly record struct MachineDowntimeId(int Value) : IEntityId<int>
{
    public static implicit operator int(MachineDowntimeId id) => id.Value;
    public static implicit operator MachineDowntimeId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
