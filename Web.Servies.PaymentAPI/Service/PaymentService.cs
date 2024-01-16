using Web.Services.PaymentAPI.Models;
using Web.Services.PaymentAPI.Models.Dto;
using Web.Services.PaymentAPI.Repository.IRepository;
using Web.Services.PaymentAPI.Service.IService;

namespace Web.Services.PaymentAPI.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService (IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }


        public PaymentDTO FindById(string id)
        {
            Payments payments = _paymentRepository.FindById(id);

            return new PaymentDTO()
            {
                paymentId = payments.id,
                isPayed = payments.isPayed,
                paymentStatus = payments.paymentStatus,
            };
        }

        public void Update(PaymentDTO paymentDTO)
        {
            Payments payments = _paymentRepository.FindById(paymentDTO.paymentId);
            if(payments != null)
            {
                payments.paymentStatus = paymentDTO.paymentStatus;
                payments.isPayed = paymentDTO.isPayed;
                _paymentRepository.Update(payments);
            } else
            {
                payments = new Payments();
                payments.paymentStatus = paymentDTO.paymentStatus;
                payments.isPayed = paymentDTO.isPayed;
                _paymentRepository.Update(payments);
            }
        }

    }
}
