using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.Models
{
    [Table("Food")]
    public class FoodModel
	{
        [Key]
        [Column(TypeName = "varchar(36)")]
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public CartModel? CartModel { get; set; }
        public OrderDetailsModel? OrderDetailsModel { get; set; }

        public FoodModel()
		{
            Id = Guid.NewGuid().ToString();
        }
	}
}

