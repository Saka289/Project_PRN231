using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Web.Services.OrderAPI.Models.Dto;
using Web.Services.OrderAPI.Service.IService;

namespace Web.Services.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderAPIController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _orderService.GetOrders();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("GetOrdersByUserId/{userId}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrdersByUserId([Required] string userId)
        {
            var result = await _orderService.GetOrdersByUserId(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder([FromBody] CartDto cartDto)
        {
            var result = await _orderService.CreateOrder(cartDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("UpdateStatus/{orderId}/{status}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateStatus([Required] string orderId, [Required] string status)
        {
            var result = await _orderService.UpdateStatus(orderId, status);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("SearchOrder/{orderId}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchOrder([Required] string orderId)
        {
            var result = await _orderService.SearchOrder(orderId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("GenerateQR/{orderId}/{amount}")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateQR([Required] string orderId, [Required] decimal amount)
        {
            var result = await _orderService.GenerateQR(orderId, amount);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
