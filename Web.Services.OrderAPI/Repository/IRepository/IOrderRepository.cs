using Web.Services.OrderAPI.Models;
using Web.Services.OrderAPI.Models.Dto;

namespace Web.Services.OrderAPI.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();

        Task<IEnumerable<Order>> GetOrdersByUserId(string userId);
        Task<Order> CreateOrder(Order order);
        Task<bool> UpdateStatus(string orderId, string status);
        Task<Order> SearchOrder(string orderId);
        void SaveChanges();
    }
}
