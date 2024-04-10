using Shared.Dtos;
using Shared.Enums;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRole(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.BaseUrlGateWay + "/api/AuthAPI/assignRole"
            });
        }

        public async Task<ResponseDto?> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = changePasswordDto,
                Url = SD.BaseUrlGateWay + "/api/AuthAPI/changePassword"
            });
        }

        public async Task<ResponseDto?> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = confirmEmailDto,
                Url = SD.BaseUrlGateWay + "/api/AuthAPI/confirmEmail"
            });
        }

        public async Task<ResponseDto?> ForgotPassword(string email)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = email,
                Url = SD.BaseUrlGateWay + "/api/AuthAPI/forgotPassword"
            });
        }

        public async Task<ResponseDto?> Login(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.BaseUrlGateWay + "/api/AuthAPI/login"
            });
        }

        public async Task<ResponseDto?> Register(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.BaseUrlGateWay + "/api/AuthAPI/register"
            });
        }

        public async Task<ResponseDto?> ResetPassword(ResetpasswordDto resetpasswordDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = resetpasswordDto,
                Url = SD.BaseUrlGateWay + "/api/AuthAPI/resetPassword"
            });
        }

        public async Task<ResponseDto?> UpdateUser(UpdateUserDto updateUserDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = updateUserDto,
                Url = SD.BaseUrlGateWay + "/api/AuthAPI/updateUser"
            });
        }
    }
}
