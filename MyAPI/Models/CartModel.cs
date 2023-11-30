using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.Models
{
    [Table("Cart")]
    public class CartModel
	{
        [Key]
        [Column(TypeName = "varchar(36)")]
        public string Id { get; set; }
        public string FoodId { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Price { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public FoodModel? FoodModel { get; set; }
        public UserModel? UserModel { get; set; }

        public CartModel()
		{
            Id = Guid.NewGuid().ToString();
        }
	}
}

