using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Web.Services.OrderAPI.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderDetailId { get; set; }
        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
