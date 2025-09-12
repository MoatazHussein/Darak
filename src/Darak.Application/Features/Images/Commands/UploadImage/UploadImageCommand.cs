using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Darak.Application.Features.Images.Commands.UploadImage;

public class UploadImageCommand : IRequest<string> 
{
    [FromForm]
    public IFormFile File { get; set; } = default!;
}
