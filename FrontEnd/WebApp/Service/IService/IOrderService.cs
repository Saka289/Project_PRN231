﻿using Shared.Dtos;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDto?> GetOrders();
        Task<ResponseDto?> CreateOrder(CartDto cartDto);
        Task<ResponseDto?> GetOrdersByUserId(string userId);
        Task<ResponseDto?> UpdateStatus(string orderId, string status);
        Task<ResponseDto?> SearchOrder(string orderId);
        Task<ResponseDto?> GenerateQR(string orderId, decimal amount);
    }
}
