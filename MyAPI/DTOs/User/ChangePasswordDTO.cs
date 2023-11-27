using System;
using System.ComponentModel.DataAnnotations;

namespace MyAPI.DTOs.User
{
	public class ChangePasswordDTO
	{
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }
        [Required, MinLength(6, ErrorMessage = "Password is required and must be at least 6 character")]
        public required string Password { get; set; }
        [Required, MinLength(6, ErrorMessage = "New password is required and must be at least 6 character")]
        public required string NewPassword { get; set; }
    }
}

