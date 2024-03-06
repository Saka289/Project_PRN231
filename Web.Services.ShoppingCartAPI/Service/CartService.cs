using Shared.Dtos;
using StackExchange.Redis;
using Web.Services.ShoppingCartAPI.Models.Dto;
using Web.Services.ShoppingCartAPI.Service.IService;

namespace Web.Services.ShoppingCartAPI.Service
{
    public class CartService : ICartService
    {
        private readonly ConnectionMultiplexer redis;
        public CartService()
        {
            redis = ConnectionMultiplexer.Connect("localhost:6379");
        }
        public Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetCart(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> RemoveCart(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> RemoveCoupon(CartDto cartDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> SaveCart(CartDto cartDto)
        {
            throw new NotImplementedException();
        }
    }
}
