namespace Web.Services.ProductAPI.Models.Dto
{
    public class ProductDtoForCreateAndUpdate
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string ProductCode { get; set; }

        public string Description { get; set; }

        public IFormFile? Image { get; set; }

        public double? Price { get; set; }

        public int? CategoryId { get; set; }

        public string? Status { get; set; }
    }
}
