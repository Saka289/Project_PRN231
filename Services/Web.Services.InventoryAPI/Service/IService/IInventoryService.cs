using System.Data;
using Web.Services.InventoryAPI.Models.Dto;

namespace Web.Services.InventoryAPI.Service.IService
{
    public interface IInventoryService
    {
        public List<InventoryDTO> isInStock(List<ProductRequest> products);
        public List<StockDto> getStock();
        public StockDto GetInventoryById(int id);
        public int Update(StockCreate stock);
        public void UpdateInventory(List<ProductRequest> products, string status);
        Task<int> Upload(IFormFile file);
    }
}


