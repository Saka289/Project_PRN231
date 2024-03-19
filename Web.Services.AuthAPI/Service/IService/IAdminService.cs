using Shared.Dtos;
using Web.Services.AuthAPI.Models.Dto;

namespace Web.Services.AuthAPI.Service.IService
{
    public interface IAdminService
    {
        Task<ResponseDto> GetMembers();
        Task<ResponseDto> GetMemberByUserID(string userId);
        Task<ResponseDto> GetMemberByEmail(string email);
        Task<ResponseDto> AddEditMember(MemberAddEditDto model);
        Task<ResponseDto> LockMember(string userId);
        Task<ResponseDto> UnlockMember(string userId);
        Task<ResponseDto> DeleteMember(string userId);
        Task<ResponseDto> GetApplicationRoles();
    }
}
