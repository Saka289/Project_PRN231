using CallService;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Enums;
using Web.Services.OrderAPI.Models.Dto;
using Web.Services.OrderAPI.Service.IService;

namespace Web.Services.OrderAPI.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ISendService _sendService;

        public CategoryService(ISendService sendService)
        {
            _sendService = sendService;
        }
        public async Task<CategoryDto> GetCategory(int categoryId)
        {
            try
            {
                var response = await _sendService.SendServiceAsync(new SendRequestDto()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ProductAPIBase + "/api/CategoryAPI/" + categoryId
                });
                if (response.IsSuccess && response.Result != null)
                {
                    var result = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString(response.Result));
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
