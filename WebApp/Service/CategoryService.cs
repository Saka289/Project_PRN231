using Shared.Dtos;
using Shared.Enums;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class CategoryService : ICategoryService
    {
        public readonly IBaseService _baseService;
        public CategoryService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateCategoryAsync(CategoryDtoForCreateAndUpdate categoryDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = categoryDto,
                Url = SD.BaseUrlGateWay + "/api/CategoryAPI",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteCategoryAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BaseUrlGateWay + "/api/CategoryAPI/" + id
            });
        }

        public async Task<ResponseDto?> DeleteSoftCategoryAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BaseUrlGateWay + "/api/CategoryAPI/DeleteSoft/" + id
            });
        }

        public async Task<ResponseDto?> GetAllCategoryAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/CategoryAPI"
            });
        }

        public async Task<ResponseDto?> GetCategoryByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/CategoryAPI/"+id
            });
        }

        public async Task<ResponseDto?> UpdateCategoryAsync(CategoryDtoForCreateAndUpdate categoryDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = categoryDto,
                Url = SD.BaseUrlGateWay + "/api/CategoryAPI/"
            });
        }
    }
}
