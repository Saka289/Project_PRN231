using Web.Services.ShoppingCartAPI.Models.Dto;

namespace Web.Services.ShoppingCartAPI.Models
{
    public class Cart
    {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
