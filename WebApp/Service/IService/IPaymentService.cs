using Shared.Dtos;
using WebApp.Models.Dtos;

namespace WebApp.Service.IService
{
    public interface IPaymentService
    {
        Task<ResponseDto?> GetPaymentById(string id);
        Task<ResponseDto?> UpsertPayment(PaymentDto paymentDto);
    }
}
