using CallService;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Enums;
using Web.Services.InventoryAPI.Models.Dto;
using Web.Services.OrderAPI.Service.IService;

namespace Web.Services.OrderAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly ISendService _sendService;

        public ProductService(ISendService sendService)
        {
            _sendService = sendService;
        }

        public async Task<List<ProductDto>> GetProduct()
        {
            try
            {
                var response = await _sendService.SendServiceAsync(new SendRequestDto()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ProductAPIBase + "/api/ProductAPI",
                });

                if (response.IsSuccess && response.Result != null)
                {
                    var result = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
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
