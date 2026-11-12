namespace FabricPlatform.API.Quality.Domain.Model.ValueObjects;

using FabricPlatform.API.Shared.Domain.Model.ValueObjects;

public readonly record struct FabricInspectionId(int Value) : IEntityId<int>;