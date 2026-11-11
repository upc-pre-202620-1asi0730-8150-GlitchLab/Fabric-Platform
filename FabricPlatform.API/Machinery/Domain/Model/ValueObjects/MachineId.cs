using FabricPlatform.API.Shared.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Machinery.Domain.Model.ValueObjects;

public readonly record struct MachineId(int Value) : IEntityId<int>
{
    public static implicit operator int(MachineId id) => id.Value;
    public static implicit operator MachineId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
