using Newtonsoft.Json.Bson;
using Web.Services.ShoppingCartAPI.Models;

namespace Web.Services.ShoppingCartAPI.Service.IService
{
    public interface CartService
    {
        CartDTO SaveCart(CartDTO cartDTO);

        CartDTO getCart(String userId);
    }
}
