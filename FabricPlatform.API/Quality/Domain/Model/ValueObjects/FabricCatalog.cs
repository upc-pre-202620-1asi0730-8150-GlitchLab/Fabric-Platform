namespace FabricPlatform.API.Quality.Domain.Model.ValueObjects;

public record FabricSupplier(int Id, string Name);

public record FabricSample(int Id, string Name, decimal ShrinkageTolerance);

public static class FabricCatalog
{
    public static IReadOnlyList<FabricSupplier> Suppliers { get; } =
    [
        new(1, "Textile A"),
        new(2, "Supplier B"),
        new(3, "Supplier C")
    ];

    public static IReadOnlyList<FabricSample> Samples { get; } =
    [
        new(1, "T-Shirt Basic", 5),
        new(2, "Hoodie", 5),
        new(3, "Crewneck", 5),
        new(4, "Leggings", 5)
    ];
}
