using MyAPI.DTOs.Food;
using MyAPI.Models;

namespace MyAPI.Cores.IRepositories
{
	public interface IFoodRepository
	{
        public Task<ICollection<FoodModel>> GetAllFood();
        public Task<FoodModel> GetFoodById(string id);
        public Task CreateFood(FoodDTO foodDTO);
        public Task UpdateFood(string id, FoodDTO foodModel);
        public Task DeleteFood(string id);
    }
}

