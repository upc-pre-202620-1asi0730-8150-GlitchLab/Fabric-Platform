namespace FabricPlatform.API.Batches.Domain.Model.ValueObjects;

public readonly record struct BatchCode
{
    public string Value { get; }

    public BatchCode(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Batch code cannot be null or whitespace.", nameof(value));
        }
        Value = value;
    }
}