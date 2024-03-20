using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Dtos
{
    public class ResetpasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
