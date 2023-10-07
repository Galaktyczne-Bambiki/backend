using BambikiBackend.AI;
using Microsoft.AspNetCore.Mvc;

namespace BambikiBackend.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AiController : ControllerBase
{
    private readonly FireRecognition _fireRecognition;

    public AiController(FireRecognition fireRecognition)
    {
        _fireRecognition = fireRecognition;
    }

    [HttpPost("/detect-fire")]
    public async Task<IActionResult> DetectFire(IFormFile file)
    {
        var result = await _fireRecognition.HasFireOnImage(file.OpenReadStream());

        return result ? Ok() : BadRequest();
    }
}