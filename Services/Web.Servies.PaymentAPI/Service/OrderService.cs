using CallService;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Enums;
using Web.Services.PaymentAPI.Models.Dto;
using Web.Services.PaymentAPI.Service.IService;

namespace Web.Services.PaymentAPI.Service
{
    public class OrderService : IOrderService
    {
        private readonly ISendService _sendService;

        public OrderService(ISendService sendService)
        {
            _sendService = sendService;
        }

        public async Task<OrderDto> GetOrder(string orderId)
        {
            try
            {
                var response = await _sendService.SendServiceAsync(new SendRequestDto()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.OrderAPIDeployBase + "/api/OrderAPI/SearchOrder/" + orderId
                });
                if (response.IsSuccess && response.Result != null)
                {
                    var result = JsonConvert.DeserializeObject<OrderDto>(Convert.ToString(response.Result));
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
