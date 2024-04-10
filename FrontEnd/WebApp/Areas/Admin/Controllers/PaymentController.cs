using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            List<PaymentDto> list = new List<PaymentDto>();
            ResponseDto? response = await _paymentService.GetPayments();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<PaymentDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
    }
}
