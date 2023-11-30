using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.Models
{
	[Table("OrderDetail")]
	public class OrderDetailsModel
	{
        [Key]
        [Column(TypeName = "varchar(36)")]
        public string Id { get; set; }
        public string FoodId { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Price { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public FoodModel? FoodModel { get; set; }
        public OrderModel? OrderModel { get; set; }
        public OrderDetailsModel()
		{
            Id = Guid.NewGuid().ToString();
		}
	}
}

