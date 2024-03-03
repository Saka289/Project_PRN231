using Web.Services.PaymentAPI.Models;
using Web.Services.PaymentAPI.Models.Dto;

namespace Web.Services.PaymentAPI.Service.IService
{
    public interface IPaymentService
    {
        public void Update(PaymentDTO paymentDTO); 

        public PaymentDTO FindById(String id);

        public Task<List<PaymentDTO>> PaymentCasso(List<PaymentCasso> payment);
    }
}
