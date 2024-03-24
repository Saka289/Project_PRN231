using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Enums;
using System.IdentityModel.Tokens.Jwt;
using WebApp.Models.Dtos;
using WebApp.Service.IService;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IAdminService _adminService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartController(ICartService cartService, IProductService productService, ICouponService couponService, IAdminService adminService, IOrderService orderService, IPaymentService paymentService, IHttpContextAccessor contextAccessor)
        {
            _cartService = cartService;
            _productService = productService;
            _couponService = couponService;
            _adminService = adminService;
            _orderService = orderService;
            _paymentService = paymentService;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBaseOnLoggedInUser());
        }

        [HttpGet]
        public async Task<IActionResult> SaveCart(int productId, int quantity, string status)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var cart = await _cartService.GetCart(userId);
            bool flag = false;
            string mess = string.Empty;
            if (cart.Result != null && cart.IsSuccess)
            {
                var cartUpdate = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(cart.Result));
                var cartDetailList = cartUpdate.CartDetails.FirstOrDefault(c => c.ProductId == productId);
                if (cartDetailList != null && status.Equals("shop"))
                {
                    cartDetailList.Quantity += quantity;
                    mess = "Add Cart Successfully !!!";
                }
                else if (cartDetailList != null && status.Equals("cart"))
                {
                    if (quantity == 0)
                    {
                        cartUpdate.CartDetails.Remove(cartDetailList);
                        mess = "Remove Cart Successfully !!!";
                        flag = true;
                    }
                    else if (cartDetailList.Quantity > quantity)
                    {
                        cartDetailList.Quantity -= 1;
                        mess = "Update Cart Successfully !!!";
                    }
                    else if (cartDetailList.Quantity < quantity)
                    {
                        cartDetailList.Quantity += 1;
                        mess = "Update Cart Successfully !!!";
                    }
                }
                else if (cartDetailList != null && status.Equals("details"))
                {
                    cartDetailList.Quantity += quantity;
                    mess = "Add Cart Successfully !!!";
                }
                else
                {
                    var product = await _productService.GetProductByIdAsync(productId);
                    if (product != null && product.IsSuccess)
                    {
                        var model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(product.Result));
                        cartUpdate.CartDetails.Add(new CartDetailsDto()
                        {
                            ProductId = productId,
                            UnitPrice = Convert.ToDecimal(model.Price),
                            Quantity = 1,
                            Product = model
                        });
                        mess = "Add Cart Successfully !!!";
                    }
                }
                var data = await _cartService.SaveCart(cartUpdate);
                if (cartUpdate.CartDetails.Count == 0)
                {
                    await _cartService.RemoveCart(userId);
                }
                return Json(new { result = data.Result, success = true, message = mess, status = flag });
            }
            else
            {
                var cartNew = new CartDto();
                var product = await _productService.GetProductByIdAsync(productId);
                if (product != null && product.IsSuccess)
                {
                    var model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(product.Result));
                    cartNew.CartHeader = new CartHeaderDto();
                    cartNew.CartDetails = new List<CartDetailsDto>();
                    cartNew.CartHeader.UserId = userId;
                    cartNew.CartDetails.Add(new CartDetailsDto()
                    {
                        ProductId = productId,
                        UnitPrice = Convert.ToDecimal(model.Price),
                        Quantity = 1,
                        Product = model
                    });
                }
                var data = await _cartService.SaveCart(cartNew);
                return Json(new { result = data.Result, success = true, message = "Add Cart Successfully !!!" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(string couponCode)
        {
            var cart = await LoadCartDtoBaseOnLoggedInUser();
            var responseCoupon = await _couponService.GetCouponByCode(couponCode);
            if (responseCoupon != null && responseCoupon.IsSuccess)
            {
                var counpon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseCoupon.Result));
                cart.CartHeader.CouponCode = counpon.CouponCode;
                cart.CartHeader.Discount = counpon.DiscountAmount;
                if (cart.CartHeader.CartTotal < counpon.MinAmount)
                {
                    TempData["error"] = "Not eligible to use discount code";
                    return RedirectToAction(nameof(CartIndex));
                }
                else
                {
                    TempData["success"] = $"Apply Coupon Discount {counpon.CouponCode} !!!";
                    await _cartService.ApplyCoupon(cart);
                    return RedirectToAction(nameof(CartIndex));
                }
            }
            else
            {
                TempData["error"] = responseCoupon.Message;
                return RedirectToAction(nameof(CartIndex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon()
        {
            var cart = await LoadCartDtoBaseOnLoggedInUser();
            if (cart != null)
            {
                cart.CartHeader.CouponCode = null;
                cart.CartHeader.Discount = 0;
                TempData["success"] = "Remove Coupon Successfully !!!";
                await _cartService.ApplyCoupon(cart);
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = "Failed Remove Coupon !!!";
                return RedirectToAction(nameof(CartIndex));
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            CheckOutViewModel checkOutViewModel = new CheckOutViewModel();
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var cart = await LoadCartDtoBaseOnLoggedInUser();
            var response = await _adminService.GetMemberByUserID(userId);
            if (response != null && response.IsSuccess)
            {
                var user = JsonConvert.DeserializeObject<MemberDto>(Convert.ToString(response.Result));
                checkOutViewModel.CartDto = cart;
                checkOutViewModel.Email = user.UserName;
                checkOutViewModel.FirstName = user.FirstName;
                checkOutViewModel.LastName = user.LastName;
                checkOutViewModel.PhoneNumber = user.PhoneNumber;
            }
            return View(checkOutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutViewModel checkOutViewModel)
        {
            var cart = await LoadCartDtoBaseOnLoggedInUser();
            if (checkOutViewModel.Status == 1)
            {
                var session = _contextAccessor.HttpContext.Session;
                cart.CartHeader.Name = checkOutViewModel.FirstName + " " + checkOutViewModel.LastName;
                cart.CartHeader.Phone = checkOutViewModel.PhoneNumber;
                cart.CartHeader.Email = checkOutViewModel.Email;
                cart.CartHeader.Address = checkOutViewModel.Address;
                cart.CartHeader.Note = checkOutViewModel.Note;
                var orderCreate = await _orderService.CreateOrder(cart);
                if (orderCreate != null && orderCreate.IsSuccess)
                {
                    TempData["success"] = orderCreate.Message;
                    var order = JsonConvert.DeserializeObject<OrderDto>(Convert.ToString(orderCreate.Result));
                    await _orderService.UpdateStatus(order.OrderId.ToString(), SD.PaymentStatus.IN_PROGRESS.ToString());
                    PaymentDto paymentDto = new PaymentDto()
                    {
                        id = Guid.Empty,
                        orderId = order.OrderIdString,
                        isPayed = true,
                        paymentStatus = SD.PaymentStatus.IN_PROGRESS,
                        refund = 0,
                    };
                    var responsePayment = await _paymentService.UpsertPayment(paymentDto);
                    if (responsePayment != null && responsePayment.IsSuccess)
                    {
                        var payment = JsonConvert.DeserializeObject<PaymentDto>(Convert.ToString(responsePayment.Result));
                        session.SetString("paymentId", payment.id.ToString());
                    }
                    session.SetString("orderId", order.OrderId.ToString());
                    return RedirectToAction(nameof(Payment));
                }
            }
            else
            {
                cart.CartHeader.Name = checkOutViewModel.FirstName + " " + checkOutViewModel.LastName;
                cart.CartHeader.Phone = checkOutViewModel.PhoneNumber;
                cart.CartHeader.Email = checkOutViewModel.Email;
                cart.CartHeader.Address = checkOutViewModel.Address;
                cart.CartHeader.Note = checkOutViewModel.Note;
                var orderCreate = await _orderService.CreateOrder(cart);
                if (orderCreate != null && orderCreate.IsSuccess)
                {
                    TempData["success"] = orderCreate.Message;
                    var order = JsonConvert.DeserializeObject<OrderDto>(Convert.ToString(orderCreate.Result));
                    await _orderService.UpdateStatus(order.OrderId.ToString(), SD.PaymentStatus.CASH.ToString());
                    TempData["orderId"] = order.OrderId;
                    return RedirectToAction(nameof(Confirmation));
                }
            }
            return View(checkOutViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            var session = _contextAccessor.HttpContext.Session;
            PaymentViewModel paymentViewModel = new PaymentViewModel();
            if (session.GetString("orderId") != null)
            {
                var response = await _orderService.SearchOrder(session.GetString("orderId"));
                if (response != null && response.IsSuccess)
                {
                    var order = JsonConvert.DeserializeObject<OrderDto>(Convert.ToString(response.Result));
                    var responseProduct = await _productService.GetAllProductAsync();
                    if (responseProduct != null && responseProduct.IsSuccess)
                    {
                        var product = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseProduct.Result));
                        foreach (var item in order.OrderDetails)
                        {
                            item.Product = product.FirstOrDefault(p => p.Id == item.ProductId);
                        }
                    }
                    var responseQR = await _orderService.GenerateQR(order.OrderIdString, order.OrderTotal);
                    paymentViewModel.OrderDto = order;
                    paymentViewModel.ImageQR = Convert.ToString(responseQR.Result);
                    TimeSpan duration = TimeSpan.FromMinutes(5);
                    session.SetString("currentTime", duration.ToString());
                    return View(paymentViewModel);
                }
            }
            TempData["error"] = "Order failed !!!";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (TempData["orderId"] != null)
            {
                var response = await _orderService.SearchOrder(TempData["orderid"].ToString());
                if (response != null && response.IsSuccess)
                {
                    var order = JsonConvert.DeserializeObject<OrderDto>(Convert.ToString(response.Result));
                    var responseProduct = await _productService.GetAllProductAsync();
                    if (responseProduct != null && responseProduct.IsSuccess)
                    {
                        var product = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseProduct.Result));
                        foreach (var item in order.OrderDetails)
                        {
                            item.Product = product.FirstOrDefault(p => p.Id == item.ProductId);
                        }
                    }
                    await _cartService.RemoveCart(userId);
                    return View(order);
                }
            }
            TempData["error"] = "Order failed !!!";
            return View();
        }

        private async Task<CartDto> LoadCartDtoBaseOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.GetCart(userId);
            if (response.Result != null)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }
    }
}
