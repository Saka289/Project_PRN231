using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Shared.Dtos;
using System.Reflection;
using System.Security.Cryptography;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductImageService _productImageService;
        public ProductController(IProductService productService, ICategoryService categoryService, IProductImageService productImageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _productImageService = productImageService;
        }
        public async Task<IActionResult> IndexAsync(string searchString)
        {
            List<ProductDto> list = new List<ProductDto>();
            if (!string.IsNullOrEmpty(searchString))
            {
                ResponseDto? response = await _productService.SearchProductAsync(searchString);
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                TempData["searchValue"] = searchString;
            }
            else
            {
                ResponseDto? response = await _productService.GetAllProductAsync();
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
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
        public async Task<IActionResult> ManageProductImage(int id)
        {
            ProductDto p = new ProductDto();
            List<ProductImageDto> productImageDtos = new List<ProductImageDto>();
            ResponseDto? response = await _productService.GetProductByIdAsync(id);
            if (response != null && response.IsSuccess)
            {
                p = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            if (p != null)
            {
                ResponseDto? response1 = await _productImageService.GetAllProductImageByProductIdAsyns(p.Id);
                if (response1 != null && response1.IsSuccess)
                {
                    productImageDtos = JsonConvert.DeserializeObject<List<ProductImageDto>>(Convert.ToString(response1.Result));
                }
                ViewBag.ProductName = p.Title;
                ViewBag.ProductId = p.Id;

            }
            return View(productImageDtos);
        }
        public async Task<IActionResult> DeleteImageOfProduct(int id)
        {
            ProductImageDto p = new ProductImageDto();
            ResponseDto? response1 = await _productImageService.GetProductImageByIdAsync(id);
            if (response1 != null && response1.IsSuccess)
            {
                p = JsonConvert.DeserializeObject<ProductImageDto>(Convert.ToString(response1.Result));
            }
            if(p != null)
            {
                ResponseDto? response = await _productImageService.DeleteProductImageAsync(id);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Delete image of product successfull";

                    return RedirectToAction(nameof(ManageProductImage), new { id = p.ProductId });
                }
                else
                {
                    TempData["error"] = response?.Message;
                    return RedirectToAction(nameof(ManageProductImage), new { id = p.ProductId });
                }
            }
            else
            {
                return NotFound();
            }
           
        }
        [HttpGet]
        public async Task<IActionResult> SetDefaultImage(int id, int pid)
        {
            ResponseDto? response = await _productImageService.ChangeDefaultImage(id, pid);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Set image to default successfull";
                return RedirectToAction(nameof(ManageProductImage), new { id = pid });
            }
            else
            {
                TempData["error"] = response?.Message;
                return RedirectToAction(nameof(ManageProductImage), new { id = pid });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadMultiImage(UploadDto modelUpload)
        {
            ResponseDto? response = await _productImageService.CreateProductImageAsync(modelUpload);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Upload image successfull";
                return RedirectToAction(nameof(ManageProductImage), new { id = modelUpload.ProductId });
            }
            else
            {
                TempData["error"] = "Upload image failed";
                return RedirectToAction(nameof(ManageProductImage), new { id = modelUpload.ProductId });
            }
        }

    }
}
