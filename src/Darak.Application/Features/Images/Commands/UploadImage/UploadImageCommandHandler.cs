using Darak.Application.Common.Interfaces;
using MediatR;

namespace Darak.Application.Features.Images.Commands.UploadImage;

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, string>
{
    private readonly IStorageService _storageService;

    public UploadImageCommandHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        return await _storageService.SaveImageAsync(request.File, cancellationToken);
    }
}
