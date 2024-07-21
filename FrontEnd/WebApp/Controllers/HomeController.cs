using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using System.Diagnostics;
using System.Linq;
using WebApp.Models;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService, IProductService productService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
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
                ViewBag.ListCate = list;
            }

            // get list Produc 
            List<ProductDto> listPAll = new List<ProductDto>();
            ResponseDto? response3 = await _productService.GetAllProductAsync();
            if (response3 != null && response3.IsSuccess)
            {
                listPAll = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response3.Result));
            }
            else
            {
                TempData["error"] = response3?.Message;
            }
            if (listPAll.Count() > 0)
            {
                var randomEightProducts = listPAll.Where(x => x.Status == "Active").OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                ViewBag.ListProduct = randomEightProducts;
            }


            // get list cate 
            List<ProductDto> listP = new List<ProductDto>();
            ResponseDto? response2 = await _productService.GetProductBestSellerAsync();
            if (response2 != null && response2.IsSuccess)
            {
                listP = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response2.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            if (listP.Count() > 0)
            {
                ViewBag.ListBestSeller = listP;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
