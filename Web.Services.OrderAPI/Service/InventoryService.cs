using Web.Services.OrderAPI.Models.Dto;
using Web.Services.OrderAPI.Service.IService;
using CallService;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Enums;

namespace Web.Services.OrderAPI.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly ISendService _sendService;

        public InventoryService(ISendService sendService)
        {
            _sendService = sendService;
        }


        public async Task<List<InventoryDto>> IsInStock(ProductRequest inventoryDto)
        {
            try
            {
                var response = await _sendService.SendServiceAsync(new SendRequestDto()
                {
                    ApiType = SD.ApiType.POST,
                    Url = SD.ProductAPIBase + "/api/Inventory/"
                });
                if (response.IsSuccess && response.Result != null)
                {
                    var result = JsonConvert.DeserializeObject<List<InventoryDto>>(Convert.ToString(response.Result));
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
