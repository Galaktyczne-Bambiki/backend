using BambikiBackend.Api.Models.FireReports;
using BambikiBackend.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BambikiBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FireReportsController : ControllerBase
{
    private readonly ReportsService _reportsService;

    public FireReportsController(ReportsService reportsService)
    {
        _reportsService = reportsService;
    }


    [HttpPost("add-report")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> AddReport([FromForm] FireReportRequestModel report, CancellationToken cancellationToken)
    {
        await _reportsService.AddReport(report, cancellationToken);

        return Ok();
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(ICollection<FireReportModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var elements = await _reportsService.GetAll(cancellationToken);
        var result = elements.Select(e => new FireReportModel()
        {
            Description = e.Description,
            Latitude = e.Latitude,
            Longitude = e.Longitude
        }).ToList();

        return Ok(result);
    }
}