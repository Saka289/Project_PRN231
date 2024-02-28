using System.ComponentModel.DataAnnotations;

namespace Web.Services.OrderAPI.Models
{
    public class Order
    {
        [Key]
        public string id { get; set; }
        [Required]
        public string customerId { get; set; }
        [Required]
        public string employeeId { get; set; }
        [Required]
        public DateTime orderDate { get; set; }
        [Required]
        public Double orderFee { get; set; }
        [Required]
        public DateTime shippedDate { get; set; }
        [Required]
        public DateTime requiredDate { get; set; }
        [Required]
        public List<OrderDetail> orderLineItemsList { get; set; }
    }
}
