using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using System.Collections.Generic;
using System.Net.WebSockets;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            List<CategoryDto> list = new List<CategoryDto>();
            ResponseDto? response = await _categoryService.GetAllCategoryAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> Update(int? id)
        {
            CategoryDto c = new CategoryDto();
            if (id != null)
            {
                //Edit
                ResponseDto? response = await _categoryService.GetAllCategoryAsync();
                if (response != null && response.IsSuccess)
                {
                    c = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString(response.Result));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return View(c); 
            }
            return View(c);
        }
        [HttpPost]
        public async Task<IActionResult> Update()
        {
            
            return View();
        }
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDtoForCreateAndUpdate model)
        {
            if (ModelState.IsValid)
            {
                //Create
                ResponseDto? response = await _categoryService.CreateCategoryAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = response?.Message;
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return View(nameof(Index));
            }
            return View();
        }
    }
}
