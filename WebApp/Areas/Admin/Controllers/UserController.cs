using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IAdminService _adminService;
        public UserController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Accounts()
        {
            IEnumerable<MemberDto> list = new List<MemberDto>();
            var response = await _adminService.GetMembers();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<IEnumerable<MemberDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemberAddEditDto memberAddEditDto)
        {
            var response = await _adminService.AddEditMember(memberAddEditDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction(nameof(Accounts));
            }
            TempData["error"] = response?.Message;
            return View(memberAddEditDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string userId)
        {
            var response = await _adminService.GetMemberByUserID(userId);
            if (response != null && response.IsSuccess)
            {
                var member = JsonConvert.DeserializeObject<MemberDto>(Convert.ToString(response.Result));
                var memberAddEdit = new MemberAddEditDto()
                {
                    UserId = member.Id,
                    Email = member.UserName,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    PhoneNumber = member.PhoneNumber,
                    Password = string.Empty
                };
                return View(memberAddEdit);
            }
            return View(new MemberAddEditDto());
        }

        [HttpPost]
        public async Task<IActionResult> Update(MemberAddEditDto memberAddEditDto)
        {
            var response = await _adminService.AddEditMember(memberAddEditDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction(nameof(Accounts));
            }
            TempData["error"] = response?.Message;
            return View(memberAddEditDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _adminService.DeleteMember(id);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction(nameof(Accounts));
            }
            TempData["error"] = response.Message;
            return RedirectToAction(nameof(Accounts));
        }

        [HttpGet]
        public async Task<IActionResult> Lock(string id)
        {
            var response = await _adminService.LockMember(id);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction(nameof(Accounts));
            }
            TempData["error"] = response.Message;
            return RedirectToAction(nameof(Accounts));
        }

        [HttpGet]
        public async Task<IActionResult> UnLock(string id)
        {
            var response = await _adminService.UnlockMember(id);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction(nameof(Accounts));
            }
            TempData["error"] = response.Message;
            return RedirectToAction(nameof(Accounts));
        }
    }
}
