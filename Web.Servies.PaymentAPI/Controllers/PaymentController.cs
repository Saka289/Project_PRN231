using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Services.PaymentAPI.Models.Dto;
using Web.Services.PaymentAPI.Service.IService;

namespace Web.Services.PaymentAPI.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpGet("/{id}")]
        public PaymentDTO findPaymentById(string id) 
        {
            return _paymentService.FindById(id);
        }

        [HttpPost]
        public void Update(PaymentDTO paymentDTO)
        {
            _paymentService.Update(paymentDTO);
        }

    }
}
