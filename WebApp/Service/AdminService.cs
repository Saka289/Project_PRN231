using Shared.Dtos;
using Shared.Enums;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class AdminService : IAdminService
    {
        private readonly IBaseService _baseService;

        public AdminService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AddEditMember(MemberAddEditDto model)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = model,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI"
            });
        }

        public async Task<ResponseDto?> DeleteMember(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI/DeleteMember/" + userId
            });
        }

        public async Task<ResponseDto?> GetApplicationRoles()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI/GetRoles"
            });
        }

        public async Task<ResponseDto?> GetMemberByEmail(string email)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI/GetMemberByUserID/" + email
            });
        }

        public async Task<ResponseDto?> GetMembers()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI"
            });
        }

        public async Task<ResponseDto?> GetMemberByUserID(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI/GetMemberByUserID/" + userId
            });
        }

        public async Task<ResponseDto?> LockMember(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = userId,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI/LockMember"
            });
        }

        public async Task<ResponseDto?> UnlockMember(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = userId,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI/UnlockMember"
            });
        }

        public async Task<ResponseDto?> UpdateRoleMember(UpdateRoleDto updateRoleDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = updateRoleDto,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI/UpdateRoleMember"
            });
        }
    }
}
