using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Services.OrderAPI.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal OrderTotal { get; set; }
        public string? CouponCode { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Discount { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        [Required]
        public DateTime ShippedDate { get; set; }
        [Required]
        public DateTime RequiredDate { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        [Required]
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
