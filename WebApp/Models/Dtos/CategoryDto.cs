using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
    }
}
