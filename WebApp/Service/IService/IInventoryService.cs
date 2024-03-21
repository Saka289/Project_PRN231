using Shared.Dtos;
using Web.Services.InventoryAPI.Models.Dto;

namespace WebApp.Service.IService
{
    public interface IInventoryService
    {
        Task<ResponseDto?> IsInStock(List<ProductCheckInventory> products);
        Task<ResponseDto?> GetAllInventory();
    }
}
