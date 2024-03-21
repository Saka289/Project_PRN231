using Shared.Dtos;
using Shared.Enums;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class ProductService : IProductService
    {
        public readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateProductAsync(ProductDtoForCreateAndUpdate model)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = model,
                Url = SD.BaseUrlGateWay + "/api/ProductAPI",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BaseUrlGateWay + "/api/ProductAPI/" + id
            });
        }

        public async Task<ResponseDto?> DeleteSoftProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BaseUrlGateWay + "/api/ProductAPI/DeleteSoft/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/ProductAPI"
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/ProductAPI/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDtoForCreateAndUpdate model)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = model,
                Url = SD.BaseUrlGateWay + "/api/ProductAPI",
                ContentType = SD.ContentType.MultipartFormData

            });
        }
    }
}
