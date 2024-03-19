namespace WebApp.Models.Dtos
{
    public class CategoryDtoForCreateAndUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public string? Status { get; set; }
    }
}
