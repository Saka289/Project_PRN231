using Web.Services.PaymentAPI.Models.Dto;

namespace Web.Services.PaymentAPI.Service.IService
{
    public interface IOrderService
    {
         Task<OrderDto> GetOrder(string orderId);
    }
}
