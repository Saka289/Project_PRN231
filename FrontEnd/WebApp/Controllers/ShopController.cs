using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public ShopController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        public async Task<IActionResult> Index(int id)
        {
            // get list cate 
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
            if (list.Count() > 0)
            {
                ViewBag.ListCate = list.Where(x => x.Status == "Active").ToList();
            }



            // get all list product
            List<ProductDto> listP = new List<ProductDto>();
            if (id > 0)
            {
                ResponseDto? response21 = await _productService.GetAllProductByCateAsync(id);
                if (response21 != null && response21.IsSuccess)
                {
                    listP = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response21.Result));
                    ViewBag.IdCate = id;
                }
                else
                {
                    TempData["error"] = response21?.Message;
                }
            }
            else
            {
                ResponseDto? response2 = await _productService.GetAllProductAsync();
                if (response2 != null && response2.IsSuccess)
                {
                    listP = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response2.Result));
                }
                else
                {
                    TempData["error"] = response2?.Message;
                }
            }
            if (listP.Count() > 0)
            {
                ViewBag.ListProduct = listP.Where(x => x.Status == "Active").ToList();
                ViewBag.ListProductCount = listP.Count;
            }



            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SearchInShopPage([FromBody] ProductSearchDto model)
        {
            List<ProductDto> listP = new List<ProductDto>();
            ResponseDto? response2 = await _productService.SearchProductInShopAsync(model);
            if (response2 != null && response2.IsSuccess)
            {
                listP = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response2.Result));
                return Json(new
                {
                    success = true,
                    data = listP.ToList(),
                });
            }
            else
            {
                TempData["error"] = response2?.Message;
                return Json(new
                {
                    success = false,
                });
            }
        }
    }
}