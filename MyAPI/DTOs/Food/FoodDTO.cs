using System;
using System.ComponentModel.DataAnnotations.Schema;
using MyAPI.Models;

namespace MyAPI.DTOs.Food
{
    public class FoodDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}

