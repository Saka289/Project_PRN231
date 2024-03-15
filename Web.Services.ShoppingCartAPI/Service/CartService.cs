using Newtonsoft.Json;
using Shared.Dtos;
using StackExchange.Redis;
using Web.Services.ShoppingCartAPI.Models.Dto;
using Web.Services.ShoppingCartAPI.Service.IService;

namespace Web.Services.ShoppingCartAPI.Service
{
    public class CartService : ICartService
    {
        private readonly ConnectionMultiplexer redis;
        protected ResponseDto _response;

        public CartService()
        {
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            _response = new ResponseDto();
        }
        public Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                var db = redis.GetDatabase();

                // Retrieve the serialized object from Redis
                string json = db.StringGet(userId + "-shopping-cart");

                if (!string.IsNullOrEmpty(json))
                {
                    // Deserialize the JSON back to the object
                    _response.Result = JsonConvert.DeserializeObject<CartDto>(json);
                    _response.IsSuccess = true;
                    _response.Message = "Shopping Cart is existed";
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public Task<ResponseDto> RemoveCart(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> RemoveCoupon(CartDto cartDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> SaveCart(CartDto cartDto)
        {
            try
            {
                IDatabase db = redis.GetDatabase();

                string json = JsonConvert.SerializeObject(cartDto);

                db.StringSet(cartDto.CartHeader.UserId + "-shopping-cart", json);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
