using Shared.Dtos;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface ICategoryService
    {
        Task<ResponseDto?> GetAllCategoryAsync();
        Task<ResponseDto?> GetCategoryByIdAsync(int id);
        Task<ResponseDto?> CreateCategoryAsync(CategoryDtoForCreateAndUpdate categoryDto);
        Task<ResponseDto?> UpdateCategoryAsync(CategoryDtoForCreateAndUpdate categoryDto);
        Task<ResponseDto?> DeleteCategoryAsync(int id);
        Task<ResponseDto?> DeleteSoftCategoryAsync(int id);
    }
}
