namespace Web.Services.ShoppingCartAPI.Models
{
    public class OrderDetailDTO
    {
        public String productCode { get; set; }
        public Double price { get; set; }
        public int quantity { get; set; }
    }
}
