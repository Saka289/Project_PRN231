using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Services.OrderAPI.Models.Dto
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public string OrderIdString
        {
            get { return OrderId.ToString("N"); }
            set { OrderId = Guid.Parse(value); }
        }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string PaymentStatus { get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
