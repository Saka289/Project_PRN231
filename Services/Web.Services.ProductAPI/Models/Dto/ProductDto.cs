using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web.Services.ProductAPI.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string ProductCode { get; set; }

        public string Description { get; set; }

        public string? Image { get; set; }

        public double? Price { get; set; }

        public int? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? Status { get; set; }
    }
}
