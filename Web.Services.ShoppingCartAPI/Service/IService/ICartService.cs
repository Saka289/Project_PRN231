using Shared.Dtos;
using Web.Services.ShoppingCartAPI.Models.Dto;

namespace Web.Services.ShoppingCartAPI.Service.IService
{
    public interface ICartService 
    {
        Task<ResponseDto> GetCart(string userId);
        Task<ResponseDto> SaveCart(CartDto cartDto);
        Task<ResponseDto> RemoveCart(string userId);
        Task<ResponseDto> ApplyCoupon(CartDto cartDto);
    }
}
