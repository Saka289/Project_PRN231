using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Enums;
using System.Security.Claims;
using WebApp.Models.Dtos;
using WebApp.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IOrderService _orderService;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider, IAdminService adminService, IOrderService orderService)
        {
            _authService = authService;
            _adminService = adminService;
            _tokenProvider = tokenProvider;
            _orderService = orderService;

        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.Login(loginRequestDto);
                if (response != null && response.IsSuccess)
                {
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));
                    await SignInUser(loginResponse);
                    _tokenProvider.SetToken(loginResponse.Token);
                    TempData["success"] = response.Message;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            if (ModelState.IsValid)
            {
                if (!registrationRequestDto.Password.Equals(registrationRequestDto.ConfirmPassword))
                {
                    TempData["warning"] = "Your password and confirmation password do not match.";
                    return View(registrationRequestDto);
                }
                var response = await _authService.Register(registrationRequestDto);
                if (response != null && response.IsSuccess)
                {
                    registrationRequestDto.Role = SD.RoleCustomer;
                    await _authService.AssignRole(registrationRequestDto);
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(registrationRequestDto);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var response = await _authService.ConfirmEmail(confirmEmailDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _adminService.GetMemberByUserID(userId);
            var viewModel = new MyAccountViewModel();
            if (response != null && response.IsSuccess)
            {
                var responseUser = JsonConvert.DeserializeObject<MemberDto>(Convert.ToString(response.Result));
                var responseOrder = await _orderService.GetOrdersByUserId(userId);
                if (responseOrder != null && responseOrder.IsSuccess)
                {
                    var order = JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(Convert.ToString(responseOrder.Result));
                    viewModel.Order = order;
                    viewModel.Member = responseUser;
                }
                return View(viewModel);
            }
            return View(new MyAccountViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> MyAccount(MyAccountViewModel myAccountViewModel)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (myAccountViewModel.Member != null)
            {
                var user = new UpdateUserDto()
                {
                    UserId = userId,
                    Email = myAccountViewModel.Member.UserName,
                    FirstName = myAccountViewModel.Member.FirstName,
                    LastName = myAccountViewModel.Member.LastName,
                    PhoneNumber = myAccountViewModel.Member.PhoneNumber,
                };
                var response = await _authService.UpdateUser(user);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    await HttpContext.SignOutAsync();
                    _tokenProvider.ClearToken();
                    return RedirectToAction(nameof(Login));
                }
                TempData["error"] = response.Message;
            }
            else if (myAccountViewModel.ChangePassword != null)
            {
                var response = await _authService.ChangePassword(myAccountViewModel.ChangePassword);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    await HttpContext.SignOutAsync();
                    _tokenProvider.ClearToken();
                    return RedirectToAction(nameof(Login));
                }
                TempData["error"] = response.Message;
                var result = await _adminService.GetMemberByUserID(userId);
                if (result != null && result.IsSuccess)
                {
                    var responseUser = JsonConvert.DeserializeObject<MemberDto>(Convert.ToString(result.Result));
                    myAccountViewModel.Member = responseUser;
                }
            }
            return View(myAccountViewModel);
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var response = await _authService.ForgotPassword(forgetPasswordDto.Email);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Check Your Mail !!!";
            }
            return View(forgetPasswordDto);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            TempData["token"] = token;
            TempData["email"] = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetpasswordDto resetpasswordDto)
        {
            if (TempData["token"] == null && TempData["email"] == null)
            {
                TempData["warning"] = "Forget Password Now.";
                return RedirectToAction(nameof(ForgetPassword));
            }
            resetpasswordDto.Token = TempData["token"].ToString();
            resetpasswordDto.Email = TempData["email"].ToString();
            if (!resetpasswordDto.NewPassword.Equals(resetpasswordDto.ConfirmNewPassword))
            {
                TempData["warning"] = "Your password and confirmation password do not match.";
                return RedirectToAction(nameof(ResetPassword), new { token = resetpasswordDto.Token, email = resetpasswordDto.Email });
            }
            var response = await _authService.ResetPassword(resetpasswordDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                await HttpContext.SignOutAsync();
                _tokenProvider.ClearToken();
                return RedirectToAction(nameof(Login));
            }
            TempData["error"] = response.Message;
            return RedirectToAction(nameof(ResetPassword), new { token = resetpasswordDto.Token, email = resetpasswordDto.Email });
        }


        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, $"{jwt.Claims.FirstOrDefault(u => u.Type == "firstname").Value} {jwt.Claims.FirstOrDefault(u => u.Type == "lastname").Value}"));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
