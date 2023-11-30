using MyAPI.DTOs.Cart;
using MyAPI.Models;

namespace MyAPI.Cores.IRepositories
{
	public interface ICartRepository
	{
        public Task<ICollection<CartModel>> GetAllCart(string id);
        public Task AddToCart(CartDTO cartDTO, string buyerId);
        public Task UpdateCart(string id, CartDTO cartDTO);
        public Task DeleteCart(string id);
    }
}

