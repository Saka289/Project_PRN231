using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web.Services.InventoryAPI.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public CategoryDto? Category { get; set; }
        public string Status { get; set; }
    }
}
