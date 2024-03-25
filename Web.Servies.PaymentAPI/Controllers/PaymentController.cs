using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Web.Services.PaymentAPI.Models.Dto;
using Web.Services.PaymentAPI.Service.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Immutable;
using System.Net;
using Microsoft.AspNetCore.Authorization;


namespace Web.Services.PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        protected ResponseDto _response;


        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _response = new ResponseDto();
        }


        [HttpGet("{id}")]
        public IActionResult findPaymentById(string id)
        {
            var payment = _paymentService.FindById(id);

            if (payment == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Payment is not existed";
                return NotFound(_response);
            }
            _response.Result = payment;
            _response.IsSuccess = true;
            _response.Message = "Payment is existed";
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        public IActionResult UpsertPayment([FromBody] PaymentDto paymentDto)
        {
            var result = _paymentService.UpsertPayment(paymentDto);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost("hanlePaymentWehook")]
        [AllowAnonymous]
        public async Task<IActionResult> hanlePaymentWehook([FromBody] PaymentWebHook paymentCasso)
        {

            var payemtn = await _paymentService.PaymentCasso(paymentCasso.data);
            if (payemtn.Any())
            {
                _response.Result = payemtn;
                _response.IsSuccess = true;
                _response.Message = "Payment is update";
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.Message = "Payment is not hanlde";
            return Ok(_response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var payemnts = await _paymentService.GetPayments();
            if (payemnts.Any())
            {
                _response.Result = payemnts;
                _response.IsSuccess = true;
                _response.Message = "List Payment";
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.Message = "Not have any Payment";
            return Ok(_response);
        }
    }
}
