using Darak.Application.Features.Images.Commands.UploadImage;
using Darak.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Darak.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadImageController(IMediator mediator) : ControllerBase
{
    [HttpPost("image")]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageCommand command)
    {
        var url = await mediator.Send(command);
        return Ok(new { url });
    }
}
