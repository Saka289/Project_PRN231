﻿using System.ComponentModel.DataAnnotations;

namespace Web.Services.AuthAPI.Models.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
