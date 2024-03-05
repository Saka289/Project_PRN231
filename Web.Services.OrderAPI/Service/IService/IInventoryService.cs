using Web.Services.OrderAPI.Models.Dto;

namespace Web.Services.OrderAPI.Service.IService
{
    public interface IInventoryService
    {
        Task<List<InventoryDto>> IsInStock(List<ProductRequest> inventoryDto);
    }
}
