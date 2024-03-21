﻿using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Dtos
{
    public class ForgetPasswordDto
    {
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}
