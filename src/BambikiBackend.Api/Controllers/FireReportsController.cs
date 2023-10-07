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
}