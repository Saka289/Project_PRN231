using Microsoft.EntityFrameworkCore;
using Web.Services.OrderAPI.Data;
using Web.Services.OrderAPI.Models;
using Web.Services.OrderAPI.Models.Dto;
using Web.Services.OrderAPI.Repository.IRepository;

namespace Web.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            Order orderCreate = _context.Orders.Add(order).Entity;
            if (orderCreate == null)
            {
                return null;
            }
            return orderCreate;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.Include(o => o.OrderDetails).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId)
        {

            var result = await _context.Orders.Include(o => o.OrderDetails).Where(r => r.UserId.Equals(userId)).ToListAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<Order> SearchOrder(string orderId)
        {
            var result = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderId.ToString().Equals(orderId) || o.OrderIdString.Equals(orderId));
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<bool> UpdateStatus(string orderId, string status)
        {
            var result = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId.ToString().Equals(orderId));
            if (result != null)
            {
                result.PaymentStatus = status;
                return true;
            }
            return false;
        }
    }
}
