using Web.Services.InventoryAPI.Models.Dto;

namespace Web.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProduct();
        Task<ProductDto> GetProductById(int id);
    }
}
