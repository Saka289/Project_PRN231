using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Shared.Dtos;
using StackExchange.Redis;
using System.Security.Principal;
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
        public async Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            try
            {
                var db = redis.GetDatabase();

                // Retrieve the serialized object from Redis
                string json = db.StringGet(cartDto.CartHeader.UserId + "-shopping-cart");

                if (!string.IsNullOrEmpty(json))
                {
                    // Deserialize the JSON back to the object
                    var cart =  JsonConvert.DeserializeObject<CartDto>(json);
                    cart.CartHeader.CouponCode = cartDto.CartHeader.CouponCode;
                    cart.CartHeader.Discount = cartDto.CartHeader.Discount;
                    db.StringSet(cartDto.CartHeader.UserId + "-shopping-cart", JsonConvert.SerializeObject(cart));
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

        public async Task<ResponseDto> RemoveCart(string userId)
        {
            try
            {
                IDatabase db = redis.GetDatabase();
                db.StringSet(userId + "-shopping-cart", string.Empty);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        public async Task<ResponseDto> SaveCart(CartDto cartDto)
        {
            try
            {
                IDatabase db = redis.GetDatabase();

                if(cartDto.CartDetails == null) 
                {
                    throw new Exception("Cart is must not null");
                }
                decimal total = 0;
                foreach (var item in cartDto.CartDetails)
                {
                    total += item.UnitPrice * item.Quantity;
                }
                cartDto.CartHeader.CartTotal = total;

                total -= cartDto.CartHeader.Discount/100 * total; 

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
