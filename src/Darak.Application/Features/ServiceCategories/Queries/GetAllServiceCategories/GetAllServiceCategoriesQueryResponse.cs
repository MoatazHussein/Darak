namespace Darak.Application.Features.ServiceCategories.Queries.GetAllServiceCategories
{
    public class GetAllServiceCategoriesQueryResponse
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = default!;
        public string NameEn { get; set; } = default!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

    }

}

