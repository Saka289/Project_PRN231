using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;
using Web.Services.AuthAPI.Models;
using Web.Services.AuthAPI.Models.Dto;
using Web.Services.AuthAPI.Service.IService;

namespace Web.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
            _response = new ResponseDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            if (await CheckEmailExistsAsync(registrationRequestDto.Email))
            {
                _response.IsSuccess = false;
                _response.Message = $"An existing account is using {registrationRequestDto.Email}, email addres. Please try with another email address";
                return BadRequest(_response);
            }
            var errorMessage = await _authService.Register(registrationRequestDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            _response.Result = true;
            _response.Message = "Registered successfully !!!";
            return Ok(_response);
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            var result = await _authService.Update(updateUserDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = await _authService.ChangePassword(changePasswordDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid username or password";
                return Unauthorized(_response);
            }

            var loginResponse = await _authService.Login(loginRequestDto);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = loginResponse.Token;
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            _response.Message = "Login successfully !!!";
            return Ok(_response);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var assignRoleSuccessful = await _authService.AssignRole(registrationRequestDto.Email, registrationRequestDto.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }
            _response.Result = true;
            _response.Message = "Assign Role successfully !!!";
            return Ok(_response);
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid email";
                return BadRequest(_response);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "This email address has not been registerd yet";
                return Unauthorized(_response);
            }

            if (user.EmailConfirmed == false)
            {
                _response.IsSuccess = false;
                _response.Message = "Please confirm your email address first.";
                return BadRequest(_response);
            }
            var result = await _authService.ForgotPassword(email);
            if (result == false)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to forgot password";
                return BadRequest(_response);
            }
            _response.Message = "Changed password successfully";
            _response.IsSuccess = true;
            _response.Result = result;
            return Ok(_response);
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetpasswordDto resetpasswordDto)
        {
            var result = await _authService.ResetPassword(resetpasswordDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "This email address has not been registerd yet";
                return Unauthorized(_response);
            }


            if (user.EmailConfirmed == true)
            {
                _response.IsSuccess = false;
                _response.Message = "Your email was confirmed before. Please login to your account";
                return BadRequest(_response);
            }

            var result = await _authService.ConfirmEmail(confirmEmailDto);
            if (result == false)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed Confirmed Email Successfully !!!";
                return BadRequest(_response);
            }
            _response.Message = "Confirmed Email successfully !!!";
            _response.IsSuccess = true;
            _response.Result = result;
            return Ok(_response);
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
