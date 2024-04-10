using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        public ProductController(IProductService productService, IProductImageService productImageService)
        {
            _productService = productService;
            _productImageService = productImageService;

        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            ProductDto p = new ProductDto();
            List<ProductImageDto> listImage = new List<ProductImageDto>();
            ResponseDto? response2 = await _productService.GetProductByIdAsync(id);
            if (response2 != null && response2.IsSuccess)
            {
                p = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response2.Result));

                if(p!= null)
                {
                    ResponseDto? response3 = await _productImageService.GetAllProductImageByProductIdAsyns(id);
                    if (response3 != null && response3.IsSuccess)
                    {
                        listImage = JsonConvert.DeserializeObject<List<ProductImageDto>>(Convert.ToString(response3.Result));
                        ViewBag.listImage = listImage;
                    }
                    else
                    {
                        TempData["error"] = response2?.Message;
                    }
                }
            }
            else
            {
                TempData["error"] = response2?.Message;
            }
            return View(p);
        }
    }
}
