namespace FabricPlatform.API.Quality.Interfaces.Rest;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FabricPlatform.API.Quality.Domain.Model.ValueObjects;

[ApiController]
[Route("api/v1")]
[Tags("Fabric-Catalog")]
public class FabricCatalogController : ControllerBase
{
    [HttpGet("suppliers")]
    [EndpointSummary("Get all fabric suppliers")]
    [ProducesResponseType(typeof(IEnumerable<FabricSupplier>), StatusCodes.Status200OK)]
    public IActionResult GetSuppliers() => Ok(FabricCatalog.Suppliers);

    [HttpGet("fabric-samples")]
    [EndpointSummary("Get all fabric samples")]
    [EndpointDescription("Get the client fabric samples with their allowed shrinkage tolerance")]
    [ProducesResponseType(typeof(IEnumerable<FabricSample>), StatusCodes.Status200OK)]
    public IActionResult GetSamples() => Ok(FabricCatalog.Samples);
}
