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
            return Ok();
        }

        [HttpPost("SaveCart")]
        public async Task<IActionResult> SaveCart([FromBody] CartDto cartDto)
        {
            return Ok();
        }

        [HttpDelete("RemoveCart")]
        public async Task<IActionResult> RemoveCart([Required] string userId)
        {
            return Ok();
        }

        [HttpPost("RemoveCart")]
        public async Task<IActionResult> ApplyCoupon([FromBody] CartDto cartDto)
        {
            return Ok();
        }

        [HttpPost("RemoveCart")]
        public async Task<IActionResult> RemoveCoupon([FromBody] CartDto cartDto)
        {
            return Ok();
        }
    }
}
