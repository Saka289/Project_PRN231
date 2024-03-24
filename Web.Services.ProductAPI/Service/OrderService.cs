
using CallService;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Enums;
using Web.Services.ProductAPI.Models.Dto;
using Web.Services.ProductAPI.Service.IService;

namespace Web.Services.OrderAPI.Service
{
    public class OrderService : IOrderService
    {
        private readonly ISendService _sendService;

        public OrderService(ISendService sendService)
        {
            _sendService = sendService;
        }

        public async Task<IEnumerable<BestSeller>> GetBestSellers()
        {
            try
            {
                var response = await _sendService.SendServiceAsync(new SendRequestDto()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.OrderAPIBase + "/api/OrderAPI/GetBessSeler"
                });
                if (response.IsSuccess && response.Result != null)
                {
                    var result = JsonConvert.DeserializeObject<IEnumerable<BestSeller>>(Convert.ToString(response.Result));
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
