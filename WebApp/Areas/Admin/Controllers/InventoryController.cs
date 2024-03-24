using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public async Task<IActionResult> Update(int id)
        {
            StockDto c = new StockDto();
            if (id != null)
            {
                //Edit
                ResponseDto? response = await _inventoryService.GetInventoryById(id);
                if (response != null && response.IsSuccess)
                {
                    c = JsonConvert.DeserializeObject<StockDto>(Convert.ToString(response.Result));
                }
                return View(c);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(StockDto model)
        {
            if (ModelState.IsValid)
            {
                //Update
                ResponseDto? response = await _inventoryService.UpdateInventoryAsync(model);
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

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ImportInvens model)
        {
            if (ModelState.IsValid)
            {
                //Update
                ResponseDto? response = await _inventoryService.ImportCsvFile(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Import inventory successfully";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


    }


}
