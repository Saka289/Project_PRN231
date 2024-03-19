using Shared.Dtos;
using Web.Services.ProductAPI.Models.Dto;

namespace Web.Services.ProductAPI.Service.IService
{
    public interface ICategoryService
    {
        Task<ResponseDto> GetAllCategories();
        ResponseDto GetCategoryById(int cateId);
        Task<ResponseDto> Add(CategoryDtoForCreateAndUpdate cateDto);
        Task<ResponseDto> Update(CategoryDtoForCreateAndUpdate cateDto);
        ResponseDto Delete(int cateId);
        ResponseDto DeleteSoft (int cateId);
    }
}
