using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using Web.Services.InventoryAPI.Models.Dto;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            List<StockDto> list = new List<StockDto>();
            ResponseDto? response = await _inventoryService.GetAllInventory();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<StockDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
    }


}
