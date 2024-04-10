using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _OrderService;

        public OrderController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            List<SelectListItem> listR = new List<SelectListItem>();
            listR.Add(new SelectListItem { Text = "NOT_STARTED", Value = "NOT_STARTED" });
            listR.Add(new SelectListItem { Text = "IN_PROGRESS", Value = "IN_PROGRESS" });
            listR.Add(new SelectListItem { Text = "COMPLETED", Value = "COMPLETED" });
            listR.Add(new SelectListItem { Text = "CASH", Value = "CASH" });
            listR.Add(new SelectListItem { Text = "REFUND", Value = "REFUND" });
            ViewBag.Status = listR;
            IEnumerable<OrderDto> list = new List<OrderDto>();
            var response = await _OrderService.GetOrders();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] OrderStatusDto orderStatusDto)
        {
            var response = await _OrderService.UpdateStatus(orderStatusDto.OrderId, orderStatusDto.Status);
            if (response != null && response.IsSuccess)
            {
                return Json(new { success = true, message = response.Message });
            }
            return Json(new { success = false, message = response.Message });
        }
    }
}
