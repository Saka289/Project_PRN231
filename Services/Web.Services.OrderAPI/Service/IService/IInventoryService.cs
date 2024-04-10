using Web.Services.OrderAPI.Models.Dto;

namespace Web.Services.OrderAPI.Service.IService
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> IsInStock(List<ProductRequest> inventoryDto);
        Task<bool> UpdateInventory(UpdateInvensRequestDto updateInvensRequestDto);
    }
}
