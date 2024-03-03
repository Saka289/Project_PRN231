using Web.Services.PaymentAPI.Models;
using Web.Services.PaymentAPI.Models.Dto;
using Web.Services.PaymentAPI.Repository.IRepository;
using Web.Services.PaymentAPI.Service.IService;
using static Shared.Enums.SD;


namespace Web.Services.PaymentAPI.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public PaymentService(IPaymentRepository paymentRepository, IUserService userService, IOrderService orderService)
        {
            _paymentRepository = paymentRepository;
            _userService = userService;
            _orderService = orderService;
        }


        public PaymentDTO FindById(string id)
        {
            Payments payments = _paymentRepository.FindById(id);

            return new PaymentDTO
            {
                paymentId = payments.id,
                isPayed = payments.isPayed,
                paymentStatus = payments.paymentStatus,
            };
        }

        public void Update(PaymentDTO paymentDTO)
        {
            Payments payments = _paymentRepository.FindById(paymentDTO.paymentId);
            if (payments != null)
            {
                payments.paymentStatus = paymentDTO.paymentStatus;
                payments.isPayed = paymentDTO.isPayed;
                _paymentRepository.Update(payments);
            }
            else
            {
                payments = new Payments();
                payments.paymentStatus = paymentDTO.paymentStatus;
                payments.isPayed = paymentDTO.isPayed;
                _paymentRepository.Update(payments);
            }
        }

        public async Task<List<PaymentDTO>> PaymentCasso(List<PaymentCasso> paymentCasso)
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
                string OrderId = destrip[indexIdOrder + 1];
                string username = destrip[indexIdOrder + 2];

                // search user name
                var user = _userService.GetUser(username);
                if (user != null)
                {
                    // search Order lấy ra amout so amout 
                    OrderDto order = await _orderService.GetOrder(OrderId);
                    Payments payments = _paymentRepository.FindByOrderId(OrderId);
                    if (payments != null && order != null)
                    {
                        if (payment.Amount == order.OrderTotal)
                        {
                            payments.paymentStatus = PaymentStatus.COMPLETED;
                        }
                        else if (payment.Amount > order.OrderTotal)
                        {
                            payments.refund = payment.Amount;
                            payments.paymentStatus = PaymentStatus.NOT_STARTED;
                        }
                        else if (payment.Amount < order.OrderTotal)
                        {
                            decimal refund = order.OrderTotal - payment.Amount;
                            payments.refund = refund;
                            payments.paymentStatus = PaymentStatus.COMPLETED;
                        }
                        _paymentRepository.Update(payments);
                        paymentDTOs.Add(new PaymentDTO
                        {
                            paymentId = payments.id,
                            paymentStatus = payments.paymentStatus,
                            refund = payments.refund,
                        });
                    }
                }
            }
            return paymentDTOs;
        }

    }
}
