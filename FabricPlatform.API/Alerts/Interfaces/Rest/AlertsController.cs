using System.Net.Mime;
using FabricPlatform.API.Alerts.Application.QueryServices;
using FabricPlatform.API.Alerts.Domain.Model.Queries;
using FabricPlatform.API.Alerts.Interfaces.Rest.Resources;
using FabricPlatform.API.Alerts.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace FabricPlatform.API.Alerts.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Alerts")]
public class AlertsController(
    IAlertQueryService alertQueryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AlertResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAlerts(
        CancellationToken cancellationToken)
    {
        var query = new GetAllAlertsQuery();

        var alerts = await alertQueryService.Handle(
            query,
            cancellationToken);

        var resources = alerts.Select(
            AlertResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }
}