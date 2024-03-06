using Web.Services.ShoppingCartAPI.Models.Dto;

namespace Web.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public ProductDto Product { get; set; }
    }
}
