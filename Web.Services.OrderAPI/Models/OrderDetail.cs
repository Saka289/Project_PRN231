using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Web.Services.OrderAPI.Models
{
    public class OrderDetail
    {
        [Key]
        public string id { get; set; }
        [Required]
        public string orderId { get; set; }
        [Required]
        public string productId { get; set; }
        [Required]
        public double unitPrice { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public float discount { get; set; }
    }
}
