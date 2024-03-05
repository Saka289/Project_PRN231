namespace Web.Services.OrderAPI.Models.Dto
{
    public class CartOrderDetailDto
    {
        public Guid? OrderDetailId { get; set; }
        public Guid? OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public ProductDto? Product { get; set; }
    }
}
