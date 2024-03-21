using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection;
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

        public async Task<IActionResult> Update(int id)
        {
            CategoryDto c = new CategoryDto();
            if (id != null)
            {
                //Edit
                ResponseDto? response = await _categoryService.GetCategoryByIdAsync(id);
                if (response != null && response.IsSuccess)
                {
                    c = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString(response.Result));
                }
                ViewBag.Category = c;

                return View(); 
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryDtoForCreateAndUpdate model)
        {
            if (ModelState.IsValid)
            {
                //Update
                ResponseDto? response = await _categoryService.UpdateCategoryAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Update category successfully";
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
        public async Task<IActionResult> Delete(int id)
        {
            if (id != null)
            {
                ResponseDto? response = await _categoryService.DeleteSoftCategoryAsync(id);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Delete category successfully";
                }
                else
                {
                    TempData["error"] = "Delete category failed";
                }
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
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
                    TempData["success"] = "Create category successfully";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
