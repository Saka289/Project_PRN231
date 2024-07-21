namespace WebApp.Models.Dtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = new CartHeaderDto();
        public List<CartDetailsDto>? CartDetails { get; set; } = new List<CartDetailsDto>();
    }
}
