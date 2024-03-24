using Microsoft.EntityFrameworkCore;
using Web.Services.OrderAPI.Data;
using Web.Services.OrderAPI.Models;
using Web.Services.OrderAPI.Models.Dto;
using Web.Services.OrderAPI.Repository.IRepository;

namespace Web.Services.OrderAPI.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly AppDbContext _context;

        public OrderDetailRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BestSeller>> GetOrderDetailRepositoryAsync()
        {
            return _context.OrderDetails
            .GroupBy(od => od.ProductId)  
            .Select(g => new BestSeller {
                ProductId = g.Key,  
                NumberOrder = g.Sum(od => od.Quantity)  
            }).OrderByDescending(item => item.NumberOrder).Take(4).ToList();
        }
    }
}
