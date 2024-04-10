using System.ComponentModel.DataAnnotations;

namespace Web.Services.InventoryAPI.Models.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string? Status { get; set; }
        public virtual ICollection<ProductDto> Products { get; set; }
    }
}
