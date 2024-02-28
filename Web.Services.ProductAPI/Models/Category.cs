using System.ComponentModel.DataAnnotations;

namespace Web.Services.ProductAPI.Models
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        
        public string? Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }


    }
}
