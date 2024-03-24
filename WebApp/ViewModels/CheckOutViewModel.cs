using System.ComponentModel.DataAnnotations;
using WebApp.Models.Dtos;

namespace WebApp.ViewModels
{
    public class CheckOutViewModel
    {
        public CartDto CartDto { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Note { get; set; }
        public Guid? paymentId { get; set; }
    }
}
