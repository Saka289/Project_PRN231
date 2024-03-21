using Shared.Dtos;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface IAdminService
    {
        Task<ResponseDto?> GetMembers();
        Task<ResponseDto?> GetMemberByUserID(string userId);
        Task<ResponseDto?> GetMemberByEmail(string email);
        Task<ResponseDto?> AddEditMember(MemberAddEditDto model);
        Task<ResponseDto?> LockMember(string userId);
        Task<ResponseDto?> UnlockMember(string userId);
        Task<ResponseDto?> DeleteMember(string userId);
        Task<ResponseDto?> GetApplicationRoles();
    }
}
