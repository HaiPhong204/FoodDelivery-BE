using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.Models
{
    [Table("Order")]
    public class OrderModel
	{
        [Key]
        [Column(TypeName = "varchar(36)")]
        public string Id { get; set; }
        public int Surcharge { get; set; }
        public double TotalPrice { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public ICollection<OrderDetailsModel>? OrderDetails { get; set; }
        public UserModel? UserModel { get; set; }

        public OrderModel()
		{
            Id = Guid.NewGuid().ToString();
        }
    }
}

