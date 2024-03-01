namespace Web.Services.OrderAPI.Models.Dto
{
    public class CartDto
    {
        public CartOrderDto CartOrder { get; set; }
        public IEnumerable<CartOrderDetailDto>? CartOrderDetails { get; set; }
    }
}
