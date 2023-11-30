using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Attributes;
using MyAPI.Cores.IRepositories;
using MyAPI.DTOs;
using MyAPI.DTOs.Food;
using MyAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("api/food")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository _foodRepo;
        private readonly IMapper _mapper;

        public FoodController(IMapper mapper, IFoodRepository food)
        {
            _foodRepo = food;
            _mapper = mapper;
        }

        [HttpGet]
        [Protect]
        [Produces(typeof(ApiResponse<FoodModel>))]
        public async Task<IActionResult> GetFood()
        {
            var foods = await _foodRepo.GetAllFood();
            return Ok(new ApiResponse<ICollection<FoodModel>>(foods, "Get Food successfully"));
        }

        [HttpGet("{id}")]
        [Protect]
        [Produces(typeof(ApiResponse<FoodModel>))]
        public async Task<IActionResult> GetFoodById(string id)
        {
            var food = await _foodRepo.GetFoodById(id);
            return Ok(new ApiResponse<FoodModel>(food, "Get Food successfully"));
        }

        [HttpPost]
        [Protect]
        [Produces(typeof(ApiResponse<FoodModel>))]
        public async Task<IActionResult> CreateFood([FromBody] FoodDTO food)
        {
            await _foodRepo.CreateFood(food);
            return Ok("Create food successfully!");
        }

        [HttpPut("{id}")]
        [Protect]
        public async Task<IActionResult> UpdateFood(string id, [FromBody] FoodDTO food)
        {
            await _foodRepo.UpdateFood(id, food);
            return Ok("Update food successfully!");
        }

        [HttpDelete("{id}")]
        [Protect]
        public async Task<IActionResult> DeleteFood(string id)
        {
            await _foodRepo.DeleteFood(id);
            return Ok("Delete food successfully!");
        }
    }
}