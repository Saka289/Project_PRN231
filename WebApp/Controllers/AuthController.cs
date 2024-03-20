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

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider, IAdminService adminService)
        {
            _authService = authService;
            _adminService = adminService;
            _tokenProvider = tokenProvider;

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
            var response = await _adminService.GetUserById(userId);
            if (response != null && response.IsSuccess)
            {
                var responseUser = JsonConvert.DeserializeObject<MemberDto>(Convert.ToString(response.Result));
                return View(responseUser);
            }
            return View(new MemberDto());
        }

        [HttpPost]
        public async Task<IActionResult> MyAccount(MemberDto memberDto)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var user = new UpdateUserDto()
            {
                UserId = userId,
                Email = memberDto.UserName,
                FirstName = memberDto.FirstName,
                LastName = memberDto.LastName,
                PhoneNumber = memberDto.PhoneNumber,
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
            return View(memberDto);
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

    }
}
