using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;

namespace MyAPI.Models
{
	[Table("User")]
	public class UserModel
    {
        [Key]
        [Column(TypeName = "varchar(36)")]
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public ICollection<CartModel>? Carts { get; set; }
        public ICollection<OrderModel>? Orders { get; set; }

        public UserModel()
		{
            Id = Guid.NewGuid().ToString();
            IsVerified = false;
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.Property(u => u.Avatar).HasDefaultValue("https://t4.ftcdn.net/jpg/02/23/50/73/360_F_223507349_F5RFU3kL6eMt5LijOaMbWLeHUTv165CB.jpg");
        }
    }
}

