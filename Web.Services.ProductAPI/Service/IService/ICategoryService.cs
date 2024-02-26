using Shared.Dtos;
using Web.Services.ProductAPI.Models.Dto;

namespace Web.Services.ProductAPI.Service.IService
{
    public interface ICategoryService
    {
        Task<ResponseDto> GetAllCategories();
        ResponseDto GetCategoryById(int cateId);
        ResponseDto Add(CategoryDtoForCreateAndUpdate cateDto);
        ResponseDto Update(CategoryDtoForCreateAndUpdate cateDto);
        ResponseDto Delete(int cateId);
        ResponseDto DeleteSoft (int cateId);
    }
}
