using System.Net.Mime;
using FabricPlatform.API.Reporting.Application.QueryServices;
using FabricPlatform.API.Reporting.Domain.Model.Queries;
using FabricPlatform.API.Reporting.Interfaces.Rest.Resources;
using FabricPlatform.API.Reporting.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace FabricPlatform.API.Reporting.Interfaces.Rest;

[ApiController]
[Route("api/v1/dashboard-metrics")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Dashboard Metrics")]
public class DashboardMetricsController(IDashboardMetricsQueryService dashboardMetricsQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get the dashboard metrics of a date")]
    [EndpointDescription("Get the consolidated production and compliance indicators of a date. When there are no records for the date, all indicators are zero")]
    [ProducesResponseType(typeof(DashboardMetricsResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardMetrics([FromQuery] DateOnly? date,
        CancellationToken cancellationToken)
    {
        var query = new GetDashboardMetricsByDateQuery(date ?? DateOnly.FromDateTime(DateTime.Now));
        var metrics = await dashboardMetricsQueryService.Handle(query, cancellationToken);
        return Ok(DashboardMetricsResourceFromEntityAssembler.ToResourceFromEntity(metrics));
    }

    [HttpGet("history")]
    [EndpointSummary("Get the dashboard metrics history")]
    [EndpointDescription("Get the production and compliance indicators of every date with production records, from the oldest to the newest")]
    [ProducesResponseType(typeof(IEnumerable<DashboardMetricsResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardMetricsHistory(CancellationToken cancellationToken)
    {
        var history = await dashboardMetricsQueryService.Handle(new GetDashboardMetricsHistoryQuery(), cancellationToken);
        return Ok(history.Select(DashboardMetricsResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
