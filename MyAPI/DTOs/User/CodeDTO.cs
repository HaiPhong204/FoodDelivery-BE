using System;
using System.ComponentModel.DataAnnotations;

namespace MyAPI.DTOs.User
{
	public class CodeDTO
	{
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Value { get; set; } = string.Empty;
    }
}

