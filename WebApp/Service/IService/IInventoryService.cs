using Shared.Dtos;
using Web.Services.InventoryAPI.Models.Dto;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface IInventoryService
    {
        Task<ResponseDto?> IsInStock(List<ProductCheckInventory> products);
        Task<ResponseDto?> GetAllInventory();
        Task<ResponseDto?> GetInventoryById(int id);
        Task<ResponseDto?> UpdateInventoryAsync(StockDto stock);
        Task<ResponseDto?> ImportCsvFile(ImportInvens model);
    }
}
