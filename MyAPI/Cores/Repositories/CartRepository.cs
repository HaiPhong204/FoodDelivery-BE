using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAPI.Cores.IRepositories;
using MyAPI.DTOs.Cart;
using MyAPI.Error;
using MyAPI.Models;

namespace MyAPI.Cores.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IMapper _mapper;
        public DataContext _context { get; set; }

        public CartRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddToCart(CartDTO cartDTO, string buyerId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.FoodId == cartDTO.FoodId);
            if(cart != null)
            {
                cart.Count++;
            } else
            {
                var cartModel = _mapper.Map<CartModel>(cartDTO);
                cartModel.BuyerId = buyerId;
                await _context.Carts.AddAsync(cartModel);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCart(string id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                throw new ApiException("Cart not found!", 404);
            }
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<CartModel>> GetAllCart(string id)
        {
            var carts = await _context.Carts.Where(c => c.BuyerId == id).Include(c => c.FoodModel).ToListAsync();
            if (carts != null)
            {
                return carts;
            }
            else
            {
                return new List<CartModel>();
            }
        }

        public async Task UpdateCart(string id, CartDTO cartDTO)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == id);
            if (cart == null)
            {
                throw new ApiException("Food not found!", 400);
            }
            else if (cartDTO.Count == 0)
            {
                await DeleteCart(cart.Id);
                return;
            }
            cart.Count = cartDTO.Count;
            cart.Price = cartDTO.Price;
            await _context.SaveChangesAsync();
        }
    }
}