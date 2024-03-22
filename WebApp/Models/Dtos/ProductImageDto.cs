namespace WebApp.Models.Dtos
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public bool IsDefault { get; set; }
        public int? ProductId { get; set; }
    }
}
