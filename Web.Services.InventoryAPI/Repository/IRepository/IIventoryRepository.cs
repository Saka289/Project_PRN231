using Web.Services.InventoryAPI.Models.Dto;

namespace Web.Services.InventoryAPI.Repository.IRepository
{
    public interface IIventoryRepository : IDisposable
    {
        public List<InventoryDTO> isInStock(List<int> productIds);
    }
}
