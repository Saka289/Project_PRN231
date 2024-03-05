using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Services.ShoppingCartAPI.Models;
using Web.Services.ShoppingCartAPI.Service.IService;

namespace Web.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCart : ControllerBase
    {
        private readonly CartService _cartService;

        public ShoppingCart(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("/{userId}")]
        public IActionResult GetCart(String userId)
        {
            return Ok(_cartService.getCart(userId));
        }


        [HttpPost]
        public IActionResult SaveCart(CartDTO cartDTO)
        {
            return Ok(_cartService.SaveCart(cartDTO));
        }

    }
}
