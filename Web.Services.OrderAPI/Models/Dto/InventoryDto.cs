namespace Web.Services.OrderAPI.Models.Dto
{
    public class InventoryDto
    {
        public int productId { get; set; }
        public bool isInStock { get; set; }
        public bool isEnoughtQty { get; set; }
    }
}
