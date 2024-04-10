using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web.Services.OrderAPI.Models.Dto
{
    public class OrderDetailDto
    {
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }
        public int ProducId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public ProductDto? Product { get; set; }
    }
}
