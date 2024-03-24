using Web.Services.InventoryAPI.Models;
using Web.Services.InventoryAPI.Models.Dto;

namespace Web.Services.InventoryAPI.Repository.IRepository
{
    public interface IIventoryRepository : IDisposable
    {
        public List<Inventory> GetAll();
        public Inventory GetInventoryById(int id);
        public int Update(Inventory inventory);
        public Task<int> Import(List<Inventory> inventories);
        void Save();
    }
}
