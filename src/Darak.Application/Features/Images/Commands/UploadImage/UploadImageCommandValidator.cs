using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Darak.Application.Features.Images.Commands.UploadImage;

public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

    public UploadImageCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(BeAnImage).WithMessage("Only image files are allowed.")
            .Must(HaveValidSize).WithMessage("File must be 5MB or less.");
    }

    private bool BeAnImage(IFormFile file)
    {
        if (file == null) return false;

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedExtensions.Contains(ext);
    }

    private bool HaveValidSize(IFormFile file)
    {
        const long maxSizeInBytes = 5 * 1024 * 1024; // 5 MB
        return file?.Length <= maxSizeInBytes;
    }
}
