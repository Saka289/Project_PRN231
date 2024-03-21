using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Dtos
{
    public class ProductDtoForCreateAndUpdate
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]

        public string ProductCode { get; set; }
        [Required]

        public string Description { get; set; }

        public IFormFile? Image { get; set; }
        [Required]

        public double? Price { get; set; }
        [Required]

        public int? CategoryId { get; set; }

        public string? Status { get; set; }
    }
}
