using Web.Services.PaymentAPI.Models;

namespace Web.Services.PaymentAPI.Repository.IRepository
{
    public interface IPaymentRepository
    {
        public void Update(PaymentDTO paymentDTO);

        public PaymentDTO FindById(String id);

        public PaymentDTO PaymentCasso(List<PaymentCasso> payment);
    }
}
