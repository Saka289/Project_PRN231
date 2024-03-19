using Web.Services.OrderAPI.Models.Dto;

namespace Web.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<ProductDto> GetProduct(int productId);
    }
}
