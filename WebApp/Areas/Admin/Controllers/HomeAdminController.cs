using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using System.Net;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IAdminService _adminService;
        private readonly IInventoryService _inventoryService;
        private readonly IOrderService _orderService;
        public HomeAdminController(IProductService productService, ICategoryService categoryService, IAdminService adminService, IInventoryService inventoryService, IOrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _adminService = adminService;
            _inventoryService = inventoryService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            int cntProductActive = 0;
            int cntUserActive = 0;
            int cntOrderActive = 0;
            //get list product active
            List<ProductDto> list = new List<ProductDto>();
            ResponseDto? response = await _productService.GetAllProductAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }

            cntProductActive = list.Count();


            //get list user active
            IEnumerable<MemberDto> listUser = new List<MemberDto>();
            var responseUser = await _adminService.GetMembers();
            if (responseUser != null && responseUser.IsSuccess)
            {
                listUser = JsonConvert.DeserializeObject<IEnumerable<MemberDto>>(Convert.ToString(responseUser.Result));
            }

            cntUserActive = listUser.Count();


            //get list order
            IEnumerable<OrderDto> listOrder = new List<OrderDto>();
            var responseOrder = await _orderService.GetOrders();
            if (responseOrder != null && responseOrder.IsSuccess)
            {
                listOrder = JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(Convert.ToString(responseOrder.Result));
            }

            cntOrderActive = listOrder.Count();
            //get recent order
            List<OrderDto> listOrderRecent = new List<OrderDto>();
            if (listOrder.Count() < 4)
            {
                listOrderRecent = listOrder.OrderByDescending(x => x.OrderDate).ToList();
            }
            else
            {
                listOrderRecent = listOrder.OrderByDescending(x => x.OrderDate).Take(4).ToList();
            }
            //get list cate
            List<CategoryDto> listCate = new List<CategoryDto>();
            var responseCate = await _categoryService.GetAllCategoryAsync();
            if (responseCate != null && responseCate.IsSuccess)
            {
                listCate = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(responseCate.Result));
            }

            // tính tổng doanh thu
            // tính tổng doanh thu theo từng category
            decimal totalRevenue = 0;
            Dictionary<string, decimal> revenueByCategory = new Dictionary<string, decimal>();

            foreach (var item in listOrder)
            {
                // Lấy danh sách sản phẩm trong đơn hàng
                foreach (OrderDetailDto od in item.OrderDetails)
                {
                    // Lấy thông tin sản phẩm
                    var productResponse = await _productService.GetProductByIdAsync(od.ProductId);
                    if (productResponse != null && productResponse.IsSuccess)
                    {
                        var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(productResponse.Result));
                        // Lấy danh mục của sản phẩm
                        if (!revenueByCategory.ContainsKey(product.CategoryName))
                        {
                            revenueByCategory.Add(product.CategoryName, od.Quantity * od.UnitPrice);
                        }
                        else
                        {
                            revenueByCategory[product.CategoryName] += od.UnitPrice * od.Quantity;
                        }
                    }

                }
                totalRevenue += item.OrderTotal;
            }
            // get best seller  
            List<ProductDto> listPSeller = new List<ProductDto>();
            ResponseDto? response234 = await _productService.GetProductBestSellerAsync();
            if (response234 != null && response234.IsSuccess)
            {
                listPSeller = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response234.Result));
            }
            else
            {
                TempData["error"] = response234?.Message;
            }

            ViewBag.ListBestSeller = listPSeller;
            ViewBag.RevenueByCategory = revenueByCategory;
            ViewBag.cntProductActive = cntProductActive;
            ViewBag.cntUserActive = cntUserActive;
            ViewBag.cntOrderActive = cntOrderActive;
            ViewBag.listOrderRecent = listOrderRecent;
            ViewBag.Revenue = totalRevenue;

            return View();
        }






        public async Task<JsonResult> GetDataForChartRevenueByCategory()
        {

            Dictionary<string, decimal> revenueByCategory = new Dictionary<string, decimal>();
            //get list order
            IEnumerable<OrderDto> listOrder = new List<OrderDto>();
            var responseOrder = await _orderService.GetOrders();
            if (responseOrder != null && responseOrder.IsSuccess)
            {
                listOrder = JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(Convert.ToString(responseOrder.Result));
                if (listOrder != null)
                {
                    foreach (var item in listOrder)
                    {
                        // Lấy danh sách sản phẩm trong đơn hàng
                        foreach (OrderDetailDto od in item.OrderDetails)
                        {
                            // Lấy thông tin sản phẩm
                            var productResponse = await _productService.GetProductByIdAsync(od.ProductId);
                            if (productResponse != null && productResponse.IsSuccess)
                            {
                                var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(productResponse.Result));
                                // Lấy danh mục của sản phẩm
                                if (!revenueByCategory.ContainsKey(product.CategoryName))
                                {
                                    revenueByCategory.Add(product.CategoryName, od.Quantity * od.UnitPrice);
                                }
                                else
                                {
                                    revenueByCategory[product.CategoryName] += od.UnitPrice * od.Quantity;
                                }
                            }
                        }
                    }
                    var labels = revenueByCategory.Keys.ToList();
                    var data = revenueByCategory.Values.ToList();   
                    
                    return Json(new
                    {
                        success = true,
                        labels = labels,
                        data = data
                    });

                }
            }
            return Json(new { success = false, message = "Failed to retrieve data." });

        }







    }
}
