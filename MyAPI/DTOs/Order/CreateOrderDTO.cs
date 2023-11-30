using System;
using MyAPI.Models;

namespace MyAPI.DTOs.Order
{
	public class CreateOrderDTO
	{
        public int Surcharge { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}

