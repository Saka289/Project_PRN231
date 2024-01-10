using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Web.Services.InventoryAPI.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public BigInteger Quantity { get; set; }
    }
}
