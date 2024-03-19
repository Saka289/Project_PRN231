namespace Web.Services.ShoppingCartAPI.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ProductCode { get; set; }

        public string Description { get; set; }

        public string? Image { get; set; }

        public double? Price { get; set; }

        public int CategoryId { get; set; }
        public CategoryDto? Category { get; set; }
        public string Status { get; set; }
    }
}
