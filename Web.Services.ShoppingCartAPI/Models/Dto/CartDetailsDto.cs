namespace Web.Services.ShoppingCartAPI.Models.Dto
{
    public class CartDetailsDto
    {
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public ProductDto Product { get; set; }
    }
}
