using Shared.Dtos;

namespace WebApp.Service.IService
{
    public interface IAdminService
    {
        Task<ResponseDto?> GetUserById(string userId);
    }
}
