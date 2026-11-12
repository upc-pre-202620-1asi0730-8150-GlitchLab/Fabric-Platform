using FabricPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace FabricPlatform.API.Batches.Domain.Model.Aggregates;

public class ProductionOperator : AggregateRoot
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Specialty { get; private set; } = string.Empty;

    protected ProductionOperator() { }

    public ProductionOperator(string name, string specialty)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Operator name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(specialty)) throw new ArgumentException("Operator specialty is required.", nameof(specialty));

        Name = name.Trim();
        Specialty = specialty.Trim();
    }
}
