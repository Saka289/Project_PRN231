namespace Web.Services.OrderAPI.Models.Dto
{
    public class InventoryDto
    {
        public int productId { get; set; }
        public Boolean isInStock { get; set; }
        public Boolean isEnoughtQty { get; set; }
    }
}
