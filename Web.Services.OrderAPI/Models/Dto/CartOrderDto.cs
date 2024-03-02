namespace Web.Services.OrderAPI.Models.Dto
{
    public class CartOrderDto
    {
        public Guid? OrderId { get; set; }
        public string UserId { get; set; }
        public decimal OrderTotal { get; set; }
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
    }
}
