using Microsoft.AspNetCore.Mvc;
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

        public async Task<ResponseDto?> GetAllInventory()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/InventoryAPI"
            });
        }

        public async Task<ResponseDto?> GetInventoryById(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + "/api/InventoryAPI/" + id
            });
        }

        public async Task<ResponseDto?> IsInStock(List<ProductCheckInventory> products)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = products,
                Url = SD.BaseUrlGateWay + "/api/InventoryAPI"
            });
        }

        public async Task<ResponseDto?> UpdateInventoryAsync(StockDto stock)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = stock,
                Url = SD.BaseUrlGateWay + "/api/InventoryAPI"
            });
        }

        public async Task<ResponseDto?> ImportCsvFile(ImportInvens model)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = model,
                Url = SD.BaseUrlGateWay + "/api/InventoryAPI/ImportCsv",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteInventory(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BaseUrlGateWay + "/api/InventoryAPI/Delete/" + id
            });
        }
    }
}
