using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Web.Services.ProductAPI.Models.Dto
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public bool IsDefault { get; set; }
        public int? ProductId { get; set; }
    }
}
