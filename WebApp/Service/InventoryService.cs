using Shared.Dtos;
using Shared.Enums;
using Web.Services.InventoryAPI.Models.Dto;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IBaseService _baseService;

        public InventoryService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public Task<ResponseDto?> GetAllInventory()
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/InventoryAPI"
            });
        }

        public async Task<ResponseDto> IsInStock(List<ProductCheckInventory> products)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = products,
                Url = SD.BaseUrlGateWay + "/api/InventoryAPI"
            });
        }

    }
}
