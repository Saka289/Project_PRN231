namespace WebApp.Models.Dtos
{ 
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public List<CartDetailsDto>? CartDetails { get; set; }
    }
}
