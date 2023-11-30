using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAPI.Cores.IRepositories;
using MyAPI.DTOs.Food;
using MyAPI.Error;
using MyAPI.Models;

namespace MyAPI.Cores.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly IMapper _mapper;
        public DataContext _context { get; set; }

        public FoodRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<FoodModel>> GetAllFood()
        {
            var foods = await _context.Foods.ToListAsync();
            if(foods != null)
            {
                return foods;
            }
            else
            {
                return new List<FoodModel>();
            }
        }

        public async Task CreateFood(FoodDTO foodDTO)
        {
            var food = await _context.Foods.FirstOrDefaultAsync(f => f.Name == foodDTO.Name);
            if(food != null)
            {
                throw new ApiException("Food already exists!", 400);
            }
            var foodModel = _mapper.Map<FoodModel>(foodDTO);
            await _context.Foods.AddAsync(foodModel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFood(string id, FoodDTO foodDTO)
        {
            var food = await _context.Foods.FirstOrDefaultAsync(f => f.Id == id);
            if (food == null)
            {
                throw new ApiException("Food not found!", 400);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFood(string id)
        {
            var food = await _context.Foods.Where(p => p.Id == id).IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync();
            if (food == null)
            {
                throw new ApiException("Food not found!", 404);
            }
            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
        }

        public async Task<FoodModel> GetFoodById(string id)
        {
            var food = await _context.Foods.FirstOrDefaultAsync(f => f.Id == id) ?? throw new ApiException("Food not found!", 400);
            return food;
        }
    }
}

