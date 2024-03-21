﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Shared.Dtos;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            List<ProductDto> list = new List<ProductDto>();
            ResponseDto? response = await _productService.GetAllProductAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
        public async Task<IActionResult> Create()
        {
            // gọi tất cả các category ra
            List<CategoryDto> listC = new List<CategoryDto>();
            List<SelectListItem> Categories = new List<SelectListItem>();
            ResponseDto? response = await _categoryService.GetAllCategoryAsync();
            if (response != null && response.IsSuccess)
            {
                listC = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result));
                Categories = listC.Select(x => new SelectListItem
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList();
            }
            ViewBag.ListC = Categories; 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductDtoForCreateAndUpdate model)
        {
            if (ModelState.IsValid)
            {
                //Create
                ResponseDto? response = await _productService.CreateProductAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Create product successfully";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id != null)
            {
                ResponseDto? response = await _productService.DeleteSoftProductAsync(id);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Delete product successfully";
                }
                else
                {
                    TempData["error"] = "Delete product failed";
                }
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        public async Task<IActionResult> Update(int id)
        {
            ProductDto p = new ProductDto();
            if (id != null)
            {
                //Edit
                ResponseDto? response = await _productService.GetProductByIdAsync(id);
                if (response != null && response.IsSuccess)
                {
                    p = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                }
                List<SelectListItem> Categories = new List<SelectListItem>();
                ResponseDto? response2 = await _categoryService.GetAllCategoryAsync();
                if (response2 != null && response2.IsSuccess)
                {
                   var listC = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response2.Result));
                    Categories = listC.Select(x => new SelectListItem
                    {
                        Text = x.Name.ToString(),
                        Value = x.Id.ToString()
                    }).ToList();
                }
                ViewBag.ListC = Categories;
                ViewBag.Product = p;

                return View();
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDtoForCreateAndUpdate model)
        {
            if (ModelState.IsValid)
            {
                //Update
                ResponseDto? response = await _productService.UpdateProductAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Update product successfully";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            List<SelectListItem> Categories = new List<SelectListItem>();
            ResponseDto? response2 = await _categoryService.GetAllCategoryAsync();
            if (response2 != null && response2.IsSuccess)
            {
                var listC = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response2.Result));
                Categories = listC.Select(x => new SelectListItem
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList();
            }
            ViewBag.ListC = Categories;
            return View();
        }


    }
}