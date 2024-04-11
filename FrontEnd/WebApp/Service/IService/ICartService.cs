using Shared.Dtos;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCart(string userId);
        Task<ResponseDto?> SaveCart(CartDto cartDto);
        Task<ResponseDto?> RemoveCart(string userId);
        Task<ResponseDto?> ApplyCoupon(CartDto cartDto);
    }
}
