namespace FabricPlatform.API.Quality.Interfaces.Rest.Resources;

using System.Collections.Generic;

public record CreateFabricInspectionResource(
    string RollId,
    string Supplier,
    string InspectionDate,
    string Sample,
    string? Color,
    string? InitialResult,
    string? RollStatus);

public record UpdateFabricInspectionResource(
    ParametersDto? Parameters,
    List<ParameterHistoryDto>? ParameterHistory,
    string? ShrinkageTest,
    List<ShrinkageTestDto>? TestHistory);

public record ParametersDto(
    int FabricWidth,
    int FabricWeight,
    string ShadeCondition,
    string WeaveCondition,
    string SurfaceCondition,
    string DefectsDetected,
    string? Observations);

public record ParameterHistoryDto(
    string Date,
    int FabricWidth,
    int FabricWeight,
    string ShadeCondition,
    string WeaveCondition,
    string SurfaceCondition,
    string DefectsDetected,
    string? Observations);

public record ShrinkageTestDto(
    string TestDate,
    string Method,
    string SampleId,
    string LenShrinkage,
    string WidthShrinkage,
    string Result,
    string? Note);

public record FabricInspectionResponse(
    int Id,
    string RollId,
    string Supplier,
    string InspectionDate,
    string Sample,
    string Color,
    string InitialResult,
    string RollStatus,
    string ShrinkageTest,
    ParametersDto? Parameters,
    IEnumerable<ParameterHistoryDto> ParameterHistory,
    IEnumerable<ShrinkageTestDto> TestHistory);