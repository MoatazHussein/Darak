namespace Darak.Application.Common.Dtos.ServiceCategories;

public class ServiceCategoryDto
{
    public Guid Id { get; set; }
    public string NameAr { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

}

