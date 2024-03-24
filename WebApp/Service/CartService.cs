using Shared.Dtos;
using Shared.Enums;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> ApplyCoupon(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.BaseUrlGateWay + "/api/CartAPI/ApplyCoupon"
            });
        }

        public async Task<ResponseDto?> GetCart(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/CartAPI/GetCart/" + userId
            });
        }

        public async Task<ResponseDto?> RemoveCart(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BaseUrlGateWay + "/api/CartAPI/RemoveCart" + userId
            });
        }

        public async Task<ResponseDto?> SaveCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.BaseUrlGateWay + "/api/CartAPI/SaveCart"
            });
        }
    }
}
