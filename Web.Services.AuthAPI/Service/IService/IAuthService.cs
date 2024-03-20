using Shared.Dtos;
using Web.Services.AuthAPI.Models.Dto;

namespace Web.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto> Update(UpdateUserDto updateUserDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
        Task<ResponseDto> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(ResetpasswordDto resetpasswordDto);
        Task<bool> ConfirmEmail(ConfirmEmailDto model);
    }
}
