namespace FabricPlatform.API.Batches.Domain.Model.ValueObjects;

public readonly record struct StageName
{
    public string Value { get; }

    public StageName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Stage name cannot be empty.", nameof(value));
        }

        Value = value.Trim();
    }
    
    public static StageName Cutting => new("Cutting");
    public static StageName Sewing => new("Sewing");
    public static StageName Finishing => new("Finishing");
    public static StageName Completed => new("Completed");
        
    public override string ToString() => Value;

}