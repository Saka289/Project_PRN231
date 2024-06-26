﻿using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using System;
using System.Text;
using Web.Services.AuthAPI.Data;
using Web.Services.AuthAPI.Models;
using Web.Services.AuthAPI.Models.Dto;
using Web.Services.AuthAPI.Service.IService;

namespace Web.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        protected ResponseDto _response;

        public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, EmailService emailService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _response = new ResponseDto();
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            bool IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = "The password you entered is incorrect. Please try again.",
                };
            }

            if (IsEmailConfirmed == false)
            {
                await SendConfirmEmailAsync(user);
                return new LoginResponseDto()
                {
                    User = null,
                    Token = "Email is unconfirmed, please confirm it first !!!",
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.FirstName + " " + user.LastName,
                PhoneNumber = user.PhoneNumber,
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token,
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToLower(),
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _context.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.FirstName + " " + userToReturn.LastName,
                        PhoneNumber = userToReturn.PhoneNumber,
                    };

                    await SendConfirmEmailAsync(userToReturn);

                    return string.Empty;
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<ResponseDto> Update(UpdateUserDto updateUserDto)
        {
            try
            {
                var checkEmail = await _userManager.Users.AnyAsync(x => x.Email.Equals(updateUserDto.Email) && x.Id != updateUserDto.UserId);
                if (checkEmail)
                {
                    _response.Result = false;
                    _response.IsSuccess = false;
                    _response.Message = $"An existing account is using {updateUserDto.Email}, email addres. Please try with another email address";
                    return _response;
                }
                var user = await _userManager.FindByIdAsync(updateUserDto.UserId);
                if (user == null)
                {
                    _response.Result = false;
                    _response.IsSuccess = false;
                    _response.Message = "Not found !!!";
                    return _response;
                }
                user.FirstName = updateUserDto.FirstName;
                user.LastName = updateUserDto.LastName;
                user.PhoneNumber = updateUserDto.PhoneNumber;
                user.Email = updateUserDto.Email;
                user.UserName = updateUserDto.Email;
                user.EmailConfirmed = false;

                await _userManager.UpdateAsync(user);
                var userToReturn = _context.ApplicationUsers.First(u => u.UserName == updateUserDto.Email);
                await SendConfirmEmailAsync(userToReturn);

                _response.IsSuccess = true;
                _response.Result = true;
                _response.Message = "Update Successfully !!!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        public async Task<ResponseDto> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.Equals(changePasswordDto.Email));
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Result = false;
                    _response.Message = "This email address has not been registerd yet";
                    return _response;
                }
                if (!changePasswordDto.NewPassword.Equals(changePasswordDto.ConfirmNewPassword))
                {
                    _response.IsSuccess = false;
                    _response.Result = false;
                    _response.Message = "Your password and confirmation password do not match.";
                    return _response;
                }
                bool checkUser = await _userManager.CheckPasswordAsync(user, changePasswordDto.Password);
                if (checkUser == false)
                {
                    _response.IsSuccess = false;
                    _response.Result = false;
                    _response.Message = "The password you entered is incorrect.";
                    return _response;
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, changePasswordDto.NewPassword);
                if (!result.Succeeded)
                {
                    _response.IsSuccess = false;
                    _response.Result = false;
                    _response.Message = result.Errors.FirstOrDefault().Description;
                    return _response;
                }
                _response.IsSuccess = true;
                _response.Result = true;
                _response.Message = "Changed password successfully !!!";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<bool> ForgotPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    return await SendForgotPasswordEmail(user);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ResponseDto> ResetPassword(ResetpasswordDto resetpasswordDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetpasswordDto.Email);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Result = false;
                    _response.Message = "This email address has not been registerd yet";
                    return _response;
                }
                if (user.EmailConfirmed == false)
                {
                    _response.IsSuccess = false;
                    _response.Result = false;
                    _response.Message = "Please confirm your email address first.";
                    return _response;
                }
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(resetpasswordDto.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetpasswordDto.NewPassword);
                if (!result.Succeeded)
                {
                    _response.IsSuccess = false;
                    _response.Result = false;
                    _response.Message = result.Errors.FirstOrDefault().Description;
                    return _response;
                }
                _response.Message = "Reset password successfully";
                _response.IsSuccess = true;
                _response.Result = result;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        public async Task<bool> ConfirmEmail(ConfirmEmailDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                    var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                    var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
                    if (result.Succeeded)
                    {
                        return true;
                    }
                }
                return false;

            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> SendConfirmEmailAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var url = $"{_configuration["UrlBase:ClientUrl"]}/{_configuration["Email:ConfirmEmailPath"]}?token={token}&email={user.Email}";

            var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
                "<p>Please confirm your email address by clicking on the following link.</p>" +
                $"<p><a href=\"{url}\">Click here</a></p>" +
                "<p>Thank you,</p>" +
                $"<br>{_configuration["Email:ApplicationName"]}";

            var emailSend = await _emailService.SendEmail(user.Email, "Confirm your email", body);

            return emailSend;
        }

        private async Task<bool> SendForgotPasswordEmail(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_configuration["UrlBase:ClientUrl"]}/{_configuration["Email:ResetPasswordPath"]}?token={token}&email={user.Email}";

            var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
               $"<p>Username: {user.UserName}.</p>" +
               "<p>In order to reset your password, please click on the following link.</p>" +
               $"<p><a href=\"{url}\">Click here</a></p>" +
               "<p>Thank you,</p>" +
               $"<br>{_configuration["Email:ApplicationName"]}";

            var emailSend = await _emailService.SendEmail(user.Email, "Forgot password", body);

            return emailSend;
        }
    }
}
