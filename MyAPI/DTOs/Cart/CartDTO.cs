using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyAPI.Models;

namespace MyAPI.DTOs.Cart
{
	public class CartDTO
	{
        public string FoodId { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Price { get; set; }
    }
}

