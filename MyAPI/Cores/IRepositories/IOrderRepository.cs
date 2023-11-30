using MyAPI.DTOs.Order;
using MyAPI.Models;

namespace MyAPI.Cores.IRepositories
{
    public interface IOrderRepository
    {
        public Task<ICollection<OrderModel>> GetAllOrder();
        public Task<OrderModel> GetOrderById(string id);
        public Task CreateOrder(string buyerId, CreateOrderDTO createOrderDTO);
        public Task DeleteOrder(string id);
    }
}