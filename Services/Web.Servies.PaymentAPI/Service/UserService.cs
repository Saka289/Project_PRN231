using CallService;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Enums;
using Web.Services.PaymentAPI.Models.Dto;
using Web.Services.PaymentAPI.Service.IService;

namespace Web.Services.PaymentAPI.Service
{
    public class UserService : IUserService
    {
        private readonly ISendService _sendService;

        public UserService(ISendService sendService)
        {
            _sendService = sendService;
        }

        public async Task<MemberDto> GetUser(string email)
        {
            try
            {
                var response = await _sendService.SendServiceAsync(new SendRequestDto()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.AuthAPIBase + "/api/AdminAPI/GetMemberByEmail/" + email
                });
                if (response.IsSuccess && response.Result != null)
                {
                    var result = JsonConvert.DeserializeObject<MemberDto>(Convert.ToString(response.Result));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }
    }
}
