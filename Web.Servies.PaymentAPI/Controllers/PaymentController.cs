using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Web.Services.PaymentAPI.Models.Dto;
using Web.Services.PaymentAPI.Service.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Web.Services.PaymentAPI.Controllers
{
    [Route("api/[controller]")]
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
        public IActionResult Update(PaymentDto paymentDTO)
        {
            _paymentService.Update(paymentDTO);
            return Ok();
        }

        [HttpPost("hanlePaymentWehook")]
        public async Task<IActionResult> hanlePaymentWehook([FromBody] PaymentWebHook paymentCasso)
        {
            
            var payemtn = await _paymentService.PaymentCasso(paymentCasso.data);
            if (payemtn != null)
            {
                _response.IsSuccess = true;
                _response.Message = "Payment is update";
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.Message = "Payment is not hanlde";
            return NotFound(_response);
        }
    }
}
