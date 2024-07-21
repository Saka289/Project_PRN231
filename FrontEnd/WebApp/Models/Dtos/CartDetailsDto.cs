namespace WebApp.Models.Dtos
{
    public class CartDetailsDto
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 0;
        public ProductDto? Product { get; set; }
    }
}
