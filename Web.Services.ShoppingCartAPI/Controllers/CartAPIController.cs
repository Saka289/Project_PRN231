using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Web.Services.ShoppingCartAPI.Models.Dto;
using Web.Services.ShoppingCartAPI.Service.IService;

namespace Web.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartAPIController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            var result = await _cartService.GetCart(userId);
            return Ok(result);
        }

        [HttpPost("SaveCart")]
        public async Task<IActionResult> SaveCart([FromBody] CartDto cartDto)
        {
            var result = await _cartService.SaveCart(cartDto);
            return Ok(result);
        }

        [HttpDelete("RemoveCart")]
        public async Task<IActionResult> RemoveCart([Required] string userId)
        {
            return Ok();
        }

        [HttpPost("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] CartDto cartDto)
        {
            return Ok();
        }

        [HttpPost("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon([FromBody] CartDto cartDto)
        {
            return Ok();
        }
    }
}
