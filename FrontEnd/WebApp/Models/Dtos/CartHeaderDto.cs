namespace WebApp.Models.Dtos
{

    public class CartHeaderDto
    {
        public string UserId { get; set; }
        public decimal CartTotal { get; set; } 
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
    }
}
