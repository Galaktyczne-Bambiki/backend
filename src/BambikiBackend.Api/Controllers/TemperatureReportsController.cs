using BambikiBackend.Api.Controllers.Models.TemperatureReports;
using BambikiBackend.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BambikiBackend.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TemperatureReportsController : ControllerBase
{

    private readonly ReportsService _reportsService;

    public TemperatureReportsController(ReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [EnableRateLimiting("fixed")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddReport([FromBody] TemperatureReport report, CancellationToken cancellationToken)
    {

        await _reportsService.AddTemperatureReportAsync(report, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Return max temperature points for day
    /// </summary>
    /// <returns></returns>
    [HttpGet("max")]
    [ProducesResponseType(typeof(ICollection<TemperatureReportResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMaxOfTheDay([FromQuery]DateOnly date, CancellationToken cancellationToken)
    {
        var data = await _reportsService.GetAllMaxTemperatureReportsAsync(date, cancellationToken);
        var result = data.Select(e => new TemperatureReportResponse()
            {
                Date = e.Date,
                Longitude = e.Longitude,
                Latitude = e.Latitude,
                CelsiusValue = e.CelsiusValue
            })
            .ToList();

        return Ok(result);
    }

}