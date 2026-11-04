using FabricPlatform.API.Shared.Domain.Model.ValueObjects;

namespace FabricPlatform.API.Batches.Domain.Model.ValueObjects;

public readonly record struct BatchId(int Value) : IEntityId<int>
{
    public static BatchId New(int value) => new(value);
}