namespace FabricPlatform.API.Quality.Domain.Model.ValueObjects;

public record ShrinkageTestRecord(
    string TestDate,
    string Method,
    string SampleId,
    string LenShrinkage,
    string WidthShrinkage,
    string Result,
    string? Note);