using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Enums;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateOrder(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.BaseUrlGateWay + "/api/OrderAPI"
            });
        }

        public async Task<ResponseDto?> GenerateQR(string orderId, decimal amount)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + $"/api/OrderAPI/GenerateQR/{orderId}/{amount}"
            });
        }

        public async Task<ResponseDto?> GetOrders()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + $"/api/OrderAPI"
            });
        }

        public async Task<ResponseDto?> GetOrdersByUserId(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + $"/api/OrderAPI/GetOrdersByUserId/{userId}"
            });
        }

        public async Task<ResponseDto?> SearchOrder(string orderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BaseUrlGateWay + $"/api/OrderAPI/SearchOrder/{orderId}"
            });
        }

        public async Task<ResponseDto?> UpdateStatus(string orderId, string status)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Url = SD.BaseUrlGateWay + $"/api/OrderAPI/UpdateStatus/{orderId}/{status}"
            });
        }
    }
}
