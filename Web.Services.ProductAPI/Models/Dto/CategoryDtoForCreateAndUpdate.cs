namespace Web.Services.ProductAPI.Models.Dto
{
    public class CategoryDtoForCreateAndUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public string? Status { get; set; }
    }
}
