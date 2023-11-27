using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.DTOs.User
{
    [Table("User")]
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public UserDTO()
        {
            Email = string.Empty;
            Token = string.Empty;
        }
    }
}

