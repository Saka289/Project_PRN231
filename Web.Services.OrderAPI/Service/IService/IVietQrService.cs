using Web.Services.OrderAPI.Models.Dto;

namespace Web.Services.OrderAPI.Service.IService
{
    public interface IVietQrService
    {
        Task<QRDataDto> GenerateQR(string orderId, decimal amount);
    }
}
