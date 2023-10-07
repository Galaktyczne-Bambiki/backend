using BambikiBackend.AI;
using BambikiBackend.Api.Models.Ai;
using Microsoft.AspNetCore.Mvc;

namespace BambikiBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AiController : ControllerBase
{
    private readonly FireRecognition _fireRecognition;

    public AiController(FireRecognition fireRecognition)
    {
        _fireRecognition = fireRecognition;
    }

    [HttpPost("detect-fire")]
    [ProducesResponseType(typeof(FireDetectionModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> DetectFire(IFormFile file)
    {
        var result = await _fireRecognition.HasFireOnImage(file.OpenReadStream());

        return Ok(new FireDetectionModel
        {
            IsFireDetected = result
        });
    }
}