using CallService;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Enums;
using Web.Services.OrderAPI.Models.Dto;
using Web.Services.OrderAPI.Service.IService;

namespace Web.Services.OrderAPI.Service
{
    public class VietQrService : IVietQrService
    {
        private readonly ISendService _sendService;

        public VietQrService(ISendService sendService)
        {
            _sendService = sendService;
        }

        public async Task<QRDataDto> GenerateQR(string orderId, decimal amount)
        {
            try
            {
                var data = new RequestQrDto()
                {
                    accountNo = "9909090111",
                    accountName = "NGUYEN HAI NAM",
                    acqId = "970422",
                    addInfo = $"ECOMMERCE {orderId}",
                    amount = amount,
                    template = "print",
                };
                var response = await _sendService.SendServiceAsync(new SendRequestDto()
                {
                    ApiType = SD.ApiType.POST,
                    Data = data,
                    Url = "https://api.vietqr.io/v2/generate"
                });
                if (response.IsSuccess && response.Result != null)
                {
                    var result = JsonConvert.DeserializeObject<QRDataDto>(Convert.ToString(response.Result));
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
