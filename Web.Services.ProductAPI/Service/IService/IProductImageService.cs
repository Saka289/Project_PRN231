using Amazon.S3.Model;
using Shared.Dtos;

namespace Web.Services.ProductAPI.Service.IService
{
    public interface IProductImageService
    {
        Task<ResponseDto> UploadMultiImage(int idProduct, IFormFile[] files);
        Task<ResponseDto> ChangeDefaultImage(int id, int pid);
        Task<ResponseDto> DeleteImageOfProductImageAsync(int id);
        Task<ResponseDto> GetListImageOfProduct(int idProduct);
    }
}
