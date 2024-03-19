using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Services.ProductAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ProductCode { get; set; }

        public string Description { get; set; }

        public string? Image { get; set; }
        [Required]  
        
        public double? Price { get; set; }
        [Required]

        public int ? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        public string Status { get; set;}

    }
}
