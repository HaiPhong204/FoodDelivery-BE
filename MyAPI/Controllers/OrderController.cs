using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Attributes;
using MyAPI.Cores.IRepositories;
using MyAPI.DTOs;
using MyAPI.DTOs.Order;
using MyAPI.Models;

namespace MyAPI.Controllers
{
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;

        public OrderController(IMapper mapper, IOrderRepository order)
        {
            _orderRepo = order;
            _mapper = mapper;
        }

        [HttpGet]
        [Protect]
        [Produces(typeof(ApiResponse<OrderModel>))]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepo.GetAllOrder();
            return Ok(new ApiResponse<ICollection<OrderModel>>(orders, "Get Order successfully"));
        }

        [HttpGet("{id}")]
        [Protect]
        [Produces(typeof(ApiResponse<OrderModel>))]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderRepo.GetOrderById(id);
            return Ok(new ApiResponse<OrderModel>(order, "Get Order successfully"));
        }

        [HttpPost]
        [Protect]
        [Produces(typeof(ApiResponse<OrderModel>))]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
        {
            var user = HttpContext.Items["User"] as UserModel;
            await _orderRepo.CreateOrder(user!.Id, createOrderDTO);
            return Ok("Create order successfully!");
        }

        [HttpDelete("{id}")]
        [Protect]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            await _orderRepo.DeleteOrder(id);
            return Ok("Delete order successfully!");
        }
    }
}

