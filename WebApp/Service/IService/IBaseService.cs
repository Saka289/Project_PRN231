using Shared.Dtos;
using WebApp.Models;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
