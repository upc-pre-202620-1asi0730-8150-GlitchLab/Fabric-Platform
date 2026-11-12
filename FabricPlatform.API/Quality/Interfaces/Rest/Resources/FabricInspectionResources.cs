namespace FabricPlatform.API.Quality.Interfaces.Rest.Resources;

using System;
using System.Collections.Generic;

public record CreateFabricInspectionResource(
    string RollId,
    string Supplier,
    string InspectionDate,
    string Sample,
    string? Color,
    string? InitialResult,
    string? RollStatus,
    decimal? RollLength,
    string? ShadeMatch,
    string? WeaveCondition,
    string? Observation);

public record UpdateRollStatusResource(string RollStatus);

public record UpdateFabricInspectionResource(
    ParametersDto? Parameters,
    List<ParameterHistoryDto>? ParameterHistory,
    string? ShrinkageTest,
    List<ShrinkageTestDto>? TestHistory);

public record ParametersDto(
    decimal FabricWidth,
    int? FabricWeight,
    decimal? FabricLength,
    string ShadeCondition,
    string? WeaveCondition,
    string? SurfaceCondition,
    string DefectsDetected,
    string? Observations,
    DateTime? UpdatedAt = null);

public record ParameterHistoryDto(
    string Date,
    decimal FabricWidth,
    int? FabricWeight,
    decimal? FabricLength,
    string ShadeCondition,
    string? WeaveCondition,
    string? SurfaceCondition,
    string DefectsDetected,
    string? Observations);

public record ShrinkageTestDto(
    string TestDate,
    string Method,
    string SampleId,
    string LenShrinkage,
    string WidthShrinkage,
    string Result,
    string? Note,
    decimal? InitialLength = null,
    decimal? InitialWidth = null,
    decimal? FinalLength = null,
    decimal? FinalWidth = null,
    decimal? Tolerance = null);

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
    IEnumerable<ShrinkageTestDto> TestHistory,
    decimal? RollLength,
    string ShadeMatch,
    string WeaveCondition,
    string Observation);