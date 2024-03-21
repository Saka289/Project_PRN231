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
        public async Task<ResponseDto?> GetUserById(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/AdminAPI/GetMemberByUserID/" + userId
            });
        }
    }
}
