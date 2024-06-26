﻿using Web.Services.OrderAPI.Models.Dto;
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

        public async Task<IEnumerable<InventoryDto>> IsInStock(List<ProductRequest> inventoryDto)
        {
            try
            {
                var response = await _sendService.SendServiceAsync(new SendRequestDto()
                {
                    ApiType = SD.ApiType.POST,
                    Data = inventoryDto,
                    Url = SD.InventoryAPIBase + "/api/InventoryAPI"
                });
                if (response.IsSuccess && response.Result != null)
                {
                    var result = JsonConvert.DeserializeObject<IEnumerable<InventoryDto>>(Convert.ToString(response.Result));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public async Task<bool> UpdateInventory(UpdateInvensRequestDto updateInvensRequestDto)
        {
            try
            {
                var response = await _sendService.SendServiceAsync(new SendRequestDto()
                {
                    ApiType = SD.ApiType.PUT,
                    Data = updateInvensRequestDto,
                    Url = SD.InventoryAPIBase + "/api/InventoryAPI/UpdateInventory"
                });
                if (response.IsSuccess && response.Result != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }
    }
}
