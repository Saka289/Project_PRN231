using Shared.Dtos;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> Login(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> Register(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> UpdateUser(UpdateUserDto updateUserDto);
        Task<ResponseDto?> AssignRole(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> ForgotPassword(string email);
        Task<ResponseDto?> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<ResponseDto?> ResetPassword(ResetpasswordDto resetpasswordDto);
        Task<ResponseDto?> ConfirmEmail(ConfirmEmailDto confirmEmailDto);
    }
}
