using Shared.Dtos;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface IProductImageService
    {
        Task<ResponseDto?> GetAllProductImageByProductIdAsyns(int pid);
        Task<ResponseDto?> GetProductImageByIdAsync(int id);
        Task<ResponseDto?> CreateProductImageAsync(UploadDto model);
        Task<ResponseDto?> ChangeDefaultImage(int id, int pid);
        Task<ResponseDto?> DeleteProductImageAsync(int id);
    }
}
