

namespace WebApp.Models.Dtos
{
    public class StockDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public long StockQuantity { get; set; }
        public long ReservedQuantity { get; set; }
        public ProductDto? Product { get; set; }
    }
}
