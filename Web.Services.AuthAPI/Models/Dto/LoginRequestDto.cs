using System.ComponentModel.DataAnnotations;

namespace Web.Services.AuthAPI.Models.Dto
{
    public class LoginRequestDto
    {
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email address")]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
