using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        public string UserId { get; set; }
        public decimal CartTotal { get; set; }
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
    }
}
