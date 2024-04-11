using Shared.Dtos;
using Shared.Enums;
using System.Runtime.CompilerServices;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class ProductImageService : IProductImageService
    {
        private readonly IBaseService _baseService;
        public ProductImageService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> ChangeDefaultImage(int id, int pid)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = SD.BaseUrlGateWay + $"/api/ProductImage/{id}/{pid}",
            });
        }

        public async Task<ResponseDto?> CreateProductImageAsync(UploadDto model)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data= model,
                Url = SD.BaseUrlGateWay + "/api/ProductImage",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteProductImageAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BaseUrlGateWay + "/api/ProductImage/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductImageByProductIdAsyns(int pid)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/ProductImage/"+pid,
            });
        }

        public async Task<ResponseDto?> GetProductImageByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/ProductImage/GetById/"+id,
            });
        }
    }
}
