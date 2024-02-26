using Shared.Dtos;
using WebApp.Models;

namespace WebApp.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(ReestDto requestDto, bool withBearer = true);
    }
}
