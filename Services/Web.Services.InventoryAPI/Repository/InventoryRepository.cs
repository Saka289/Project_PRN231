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

        public Inventory GetInventoryById(int id)
        {
            return _context.Inventories.FirstOrDefault(x => x.Id == id);
        }

        public async Task<int> Import(List<Inventory> inventories)
        {
            await _context.Inventories.AddRangeAsync(inventories);
            return _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public int Update(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            return _context.SaveChanges();
        }

        public async Task<bool> UpdateRange(List<Inventory> inventories)
        {
            _context.UpdateRange(inventories);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
