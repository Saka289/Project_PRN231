using Shared.Dtos;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetAllProductAsync();
        Task<ResponseDto?> SearchProductAsync(string searchValue);
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDtoForCreateAndUpdate categoryDto);
        Task<ResponseDto?> UpdateProductAsync(ProductDtoForCreateAndUpdate categoryDto);
        Task<ResponseDto?> DeleteProductAsync(int id);
        Task<ResponseDto?> DeleteSoftProductAsync(int id);
    }
}
