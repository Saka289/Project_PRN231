using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using WebApp.Models.Dtos;
using WebApp.Service;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> Index()
        {
            List<CouponDto> list = new List<CouponDto>();
            ResponseDto? response = await _couponService.GetAllCoupon();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> Update(int id)
        {
            CouponDto c = new CouponDto();
            if (id != null)
            {
                //Edit
                ResponseDto? response = await _couponService.GetCouponById(id);
                if (response != null && response.IsSuccess)
                {
                    c = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                }
                return View(c);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                //Update
                ResponseDto? response = await _couponService.UpdateCounpon(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Update inventory successfully";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id != null)
            {
                ResponseDto? response = await _couponService.DeleteCouponById(id);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Delete coupon successfully";
                }
                else
                {
                    TempData["error"] = "Delete coupon failed";
                }
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
