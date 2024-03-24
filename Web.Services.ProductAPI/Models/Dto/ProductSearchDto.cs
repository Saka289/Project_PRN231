namespace Web.Services.ProductAPI.Models.Dto
{
    public class ProductSearchDto
    {
        public string? Title { get; set; }
        public int? CategoryId { get; set; }
        public double? PriceFrom { get; set; }
        public double? PriceTo { get; set; }
        public string? sortQuery { get; set; }
    }
}
