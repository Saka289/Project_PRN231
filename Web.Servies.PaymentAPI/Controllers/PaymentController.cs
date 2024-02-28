using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Web.Services.PaymentAPI.Models.Dto;
using Web.Services.PaymentAPI.Service.IService;

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


        [HttpGet("/{id}")]
        public IActionResult findPaymentById(string id) 
        {
            var payment = _paymentService.FindById(id);

            if(payment == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Payment is not existed";
                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.Message = "Payment is existed";
            return Ok(_response);
        }

        [HttpPost]
        public IActionResult Update(PaymentDTO paymentDTO)
        {
            return Ok(); 
        }

        [HttpPost("/hanlePaymentWehook")]
        public IActionResult hanlePaymentWehook(List<PaymentCasso> paymentCasso)
        {
            _response.IsSuccess = true;
            _response.Message = "Payment is update";
            _response.Result = _paymentService.PaymentCasso(paymentCasso);
            return Ok(_response);
        }
    }
}
