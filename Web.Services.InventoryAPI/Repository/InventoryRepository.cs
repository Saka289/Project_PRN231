using Web.Services.InventoryAPI.Data;
using Web.Services.InventoryAPI.Models;
using Web.Services.InventoryAPI.Models.Dto;
using Web.Services.InventoryAPI.Repository.IRepository;

namespace Web.Services.InventoryAPI.Repository
{
    public class InventoryRepository : IIventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public List<Inventory> GetAll()
        {
            
            return _context.Inventories.ToList();
        }
    }
}
