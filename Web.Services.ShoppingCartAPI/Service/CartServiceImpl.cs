using Web.Services.ShoppingCartAPI.Models;
using Web.Services.ShoppingCartAPI.Service.IService;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using Newtonsoft.Json;
using System;

namespace Web.Services.ShoppingCartAPI.Service
{
    public class CartServiceImpl : CartService
    {
        ConnectionMultiplexer redis;
        public CartServiceImpl()
        {
            redis = ConnectionMultiplexer.Connect("localhost:6379");
        }

        public CartDTO getCart(string userId)
        {
            var db = redis.GetDatabase();

            // Retrieve the serialized object from Redis
            string json = db.StringGet(userId + "-shopping-cart");

            // Deserialize the JSON back to the object
            return JsonConvert.DeserializeObject<CartDTO>(json);

        }

        public CartDTO SaveCart(CartDTO cart)
        {
            IDatabase db = redis.GetDatabase();

            string json = JsonConvert.SerializeObject(cart);

            db.StringSet(cart.userId + "-shopping-cart", json);
            return cart;
        }
    }
}
