using Web.Services.PaymentAPI.Models.Dto;

namespace Web.Services.PaymentAPI.Service.IService
{
    public interface IUserService
    {
        Task<MemberDto> GetUser(string email);
    }
}
