using System.Net.Mime;
using FabricPlatform.API.Analytics.Application.QueryServices;
using FabricPlatform.API.Analytics.Domain.Model.Queries;
using FabricPlatform.API.Analytics.Interfaces.Rest.Resources;
using FabricPlatform.API.Analytics.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace FabricPlatform.API.Analytics.Interfaces.Rest;

[ApiController]
[Route("api/v1/dashboard-metrics")]
[Produces(MediaTypeNames.Application.Json)]
public class DashboardMetricsController(
    IAnalyticsQueryService analyticsQueryService)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(
        typeof(DashboardMetricsResource),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardMetrics(
        [FromQuery] DateOnly date,
        CancellationToken cancellationToken)
    {
        var query = new GetDashboardMetricsQuery(date);

        var result = await analyticsQueryService.HandleAsync(
            query,
            cancellationToken);

        var resource =
            DashboardMetricsResourceAssembler.ToResourceFromResult(result);

        return Ok(resource);
    }
}