using Web.Services.OrderAPI.Models.Dto;

namespace Web.Services.OrderAPI.Service.IService
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategory(int categoryId);
    }
}
