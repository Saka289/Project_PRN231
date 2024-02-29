using Web.Services.PaymentAPI.Models;
using Web.Services.PaymentAPI.Models.Dto;
using Web.Services.PaymentAPI.Repository.IRepository;
using Web.Services.PaymentAPI.Service.IService;

namespace Web.Services.PaymentAPI.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserService _userService;

        public PaymentService (IPaymentRepository paymentRepository, IUserService userService)
        {
            _paymentRepository = paymentRepository;
            _userService = userService; 
        }


        public  PaymentDTO FindById(string id)
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

        public List<PaymentDTO> PaymentCasso(List<PaymentCasso> paymentCasso)
        {
            if (paymentCasso == null) { return null; }

            List<PaymentDTO> paymentDTOs = new List<PaymentDTO>();

            foreach (PaymentCasso payment in paymentCasso)
            {
                if (string.IsNullOrEmpty(payment.Description))
                {
                    throw new ArgumentNullException("Payment invalid");
                }

                string[] destrip = payment.Description.Split(" ");
                int indexIdOrder = -1;
                for (int j = 0; j < destrip.Length; j++)
                {
                    if (destrip[j].Equals("ECOMMERCE"))
                    {
                        indexIdOrder = j;
                    }
                }
                int OrderId = Int16.Parse(destrip[indexIdOrder + 1]);
                string username = destrip[indexIdOrder + 2];

                // search user name
                var user = _userService.GetUser(username);
                if (user != null)
                {
                    // search Order lấy ra amout so amout 


                    // cập nhật payment bằng cách lấy ra payment với orderId
                }




                PaymentDTO paymentDTO = null;

                paymentDTOs.Add(paymentDTO);

            }


            return paymentDTOs;
        }

    }
}
