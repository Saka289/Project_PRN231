using Web.Services.PaymentAPI.Models;
using Web.Services.PaymentAPI.Models.Dto;

namespace Web.Services.PaymentAPI.Service.IService
{
    public interface IPaymentService
    {
        public void Update(PaymentDto paymentDTO); 

        public PaymentDto FindById(string id);

        public Task<List<PaymentDto>> PaymentCasso(List<PaymentCasso> payment);
    }
}
