using System;
using System.ComponentModel.DataAnnotations;

namespace MyAPI.DTOs.User
{
	public class ResetPasswordDTO
	{
        [Required]
        public string Value { get; set; } = string.Empty;
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters, dude!")]
        public string Password { get; set; } = string.Empty;
    }
}

