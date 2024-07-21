using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Enums;
using Stripe;
using Stripe.Checkout;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Web.Services.OrderAPI.Data;
using Web.Services.OrderAPI.Models;
using Web.Services.OrderAPI.Models.Dto;
using Web.Services.OrderAPI.Service.IService;

namespace Web.Services.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;
        protected ResponseDto _response;
        private readonly IMapper _mapper;

        public OrderAPIController(IOrderService orderService, AppDbContext context, IMapper mapper)
        {
            _orderService = orderService;
            _context = context;
            _response = new ResponseDto();
            _mapper = mapper;
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


        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                var DiscountObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions()
                    {
                        Coupon = stripeRequestDto.Order.CouponCode
                    }
                };

                foreach (var item in stripeRequestDto.Order.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)item.UnitPrice,
                            Currency = "vnd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title
                            }
                        },
                        Quantity = item.Quantity
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                if (stripeRequestDto.Order.Discount > 0)
                {
                    options.Discounts = DiscountObj;
                }

                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequestDto.StripeSessionUrl = session.Url;
                Order orderHeader = _context.Orders.First(u => u.OrderId == stripeRequestDto.Order.OrderId);
                orderHeader.StripeSessionId = session.Id;
                await _context.SaveChangesAsync();
                _response.Result = stripeRequestDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] string orderId)
        {
            try
            {
                Order orderHeader = _context.Orders.First(u => u.OrderId.ToString() == orderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    //then payment was successful
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.PaymentStatus = SD.PaymentStatus.COMPLETED.ToString();
                    await _context.SaveChangesAsync();
                    _response.Result = _mapper.Map<OrderDto>(orderHeader);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<ResponseDto> UpdateOrderStatus(string orderId, [FromBody] string newStatus)
        {
            try
            {
                Order orderHeader = _context.Orders.First(u => u.OrderId.ToString() == orderId);
                if (orderHeader != null)
                {
                    if (newStatus == SD.PaymentStatus.REFUND.ToString())
                    {
                        //we will give refund
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId
                        };
                        var service = new RefundService();
                        Refund refund = service.Create(options);
                    }
                    orderHeader.PaymentStatus = newStatus;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
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
        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("GetBestSeller")]
        public async Task<IActionResult> GetBestSaler()
        {
            var result = await _orderService.GetBestSeller();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
