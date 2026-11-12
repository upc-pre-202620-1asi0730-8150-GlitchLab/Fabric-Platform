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
    public static StageName QualityCheck => new("Quality Check");
    public static StageName Completed => new("Completed");

    public static bool TryParseProductionStage(string? value, out StageName stage, out int progressPercentage)
    {
        switch (value?.Trim().ToUpperInvariant())
        {
            case "CUTTING":
                stage = Cutting;
                progressPercentage = 10;
                return true;
            case "SEWING":
                stage = Sewing;
                progressPercentage = 40;
                return true;
            case "FINISHING":
                stage = Finishing;
                progressPercentage = 60;
                return true;
            case "QUALITY CHECK":
                stage = QualityCheck;
                progressPercentage = 80;
                return true;
            case "COMPLETED":
                stage = Completed;
                progressPercentage = 100;
                return true;
            default:
                stage = default;
                progressPercentage = 0;
                return false;
        }
    }
        
    public override string ToString() => Value;

}