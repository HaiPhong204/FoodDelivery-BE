using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Attributes;
using MyAPI.Cores.IRepositories;
using MyAPI.DTOs;
using MyAPI.DTOs.Cart;
using MyAPI.Error;
using MyAPI.Models;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;

        public CartController(IMapper mapper, ICartRepository cart)
        {
            _cartRepo = cart;
            _mapper = mapper;
        }

        [HttpGet]
        [Protect]
        [Produces(typeof(ApiResponse<CartModel>))]
        public async Task<IActionResult> GetAllCart()
        {
            var user = HttpContext.Items["User"] as UserModel;
            var carts = await _cartRepo.GetAllCart(user!.Id);
            return Ok(new ApiResponse<ICollection<CartModel>>(carts, "Get order by id successfully"));
        }

        [HttpPost]
        [Protect]
        [Produces(typeof(ApiResponse<string>))]
        public async Task<IActionResult> AddToCart([FromBody] CartDTO cartDTO)
        {
            var user = HttpContext.Items["User"] as UserModel;
            await _cartRepo.AddToCart(cartDTO, user!.Id);
            return Ok(new ApiResponse<string>(string.Empty, "Add to cart successfully"));
        }

        [HttpPut("{id}")]
        [Protect]
        [Produces(typeof(ApiResponse<string>))]
        public async Task<IActionResult> UpdateCart(string id,[FromBody] CartDTO cartDTO)
        {
            await _cartRepo.UpdateCart(id, cartDTO);
            return Ok(new ApiResponse<string>(string.Empty, "Update cart successfully"));
        }

        [HttpDelete("{id}")]
        [Protect]
        [Produces(typeof(ApiResponse<string>))]
        public async Task<IActionResult> DeleteCart(string id)
        {
            await _cartRepo.DeleteCart(id);
            return Ok(new ApiResponse<string>(string.Empty, "Delete cart successfully"));
        }
    }
}