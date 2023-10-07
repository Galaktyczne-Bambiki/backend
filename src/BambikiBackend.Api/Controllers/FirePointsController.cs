using BambikiBackend.Api.Controllers.Models.FirePoints;
using BambikiBackend.Api.Integrations.Firms;
using BambikiBackend.Api.Integrations.Firms.Models;
using Microsoft.AspNetCore.Mvc;

namespace BambikiBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FirePointsController : ControllerBase
{
    private readonly IAreaRestClient _areaRestClient;

    public FirePointsController(IAreaRestClient areaRestClient)
    {
        _areaRestClient = areaRestClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] AreaCords cords, [FromQuery] DateOnly date, CancellationToken cancellationToken)
    {
        var data = await _areaRestClient.GetAreaData(date, cords, cancellationToken);
        var result = data.Select(e => new FirePointsModel(e)).ToList();
        return Ok(result);
    }
}