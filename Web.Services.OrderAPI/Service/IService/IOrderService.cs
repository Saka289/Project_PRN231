using Shared.Dtos;
using Web.Services.OrderAPI.Models;
using Web.Services.OrderAPI.Models.Dto;

namespace Web.Services.OrderAPI.Service.IService
{
    public interface IOrderService 
    {
        Task<ResponseDto> GetOrders();
        Task<ResponseDto> GetOrdersByUserId(string userId);
        Task<ResponseDto> CreateOrder(CartDto cartDto);
        Task<ResponseDto> UpdateStatus(string orderId, string status);
        Task<ResponseDto> SearchOrder(string orderId);
    }
}
