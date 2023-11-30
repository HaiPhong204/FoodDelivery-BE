using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAPI.Cores.IRepositories;
using MyAPI.DTOs.Order;
using MyAPI.Error;
using MyAPI.Models;

namespace MyAPI.Cores.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMapper _mapper;
        public DataContext _context { get; set; }

        public OrderRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<OrderModel>> GetAllOrder()
        {
            var orders = await _context.Orders.Include(o => o.OrderDetails).ToListAsync();
            if (orders != null)
            {
                return orders;
            }
            else
            {
                return new List<OrderModel>();
            }
        }

        public async Task<OrderModel> GetOrderById(string id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .Include(u => u.UserModel)
                .FirstOrDefaultAsync(f => f.Id == id) ?? throw new ApiException("Order not found!", 400);
            return order;
        }

        public async Task CreateOrder(string buyerId, CreateOrderDTO createOrderDTO)
        {
            var carts = await _context.Carts.Where(c => c.BuyerId == buyerId).Include(c => c.FoodModel).ToListAsync();
            if (carts == null)
            {
                throw new ApiException("Cart is empty!", 400);
            }

            double ttPrice = carts.Sum(item => item.Price * item.Count);

            //Create List<OrderDetails>
            var orderDetails = carts.Select(cart => new OrderDetailsModel
            {
                FoodId = cart.FoodId,
                Count = cart.Count,
                Price = cart.Price,
            }).ToList();

            //Create Order contain List<OrderDetails>
            var order = new OrderModel
            {
                TotalPrice = ttPrice,
                Status = createOrderDTO.Status,
                Surcharge = createOrderDTO.Surcharge,
                Notes = createOrderDTO.Notes,
                BuyerId = buyerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            orderDetails.ForEach(item => item.OrderId = order.Id);

            await _context.OrderDetails.AddRangeAsync(orderDetails);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(string id)
        {
            var order = await _context.Orders.Where(p => p.Id == id).IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync();
            if (order == null)
            {
                throw new ApiException("Order not found!", 404);
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}