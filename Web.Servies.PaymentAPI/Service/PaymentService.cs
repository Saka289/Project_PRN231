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


        public PaymentDto FindById(string id)
        {
            Payments payments = _paymentRepository.FindById(id);

            if (payments != null)
            {
                PaymentDto paymentDto = new PaymentDto
                {
                    paymentId = payments.id,
                    orderId = payments.orderId,
                    isPayed = payments.isPayed,
                    paymentStatus = payments.paymentStatus,
                };
            }
            return null;
        }

        public void Update(PaymentDto paymentDTO)
        {
            Payments payments = _paymentRepository.FindById(paymentDTO.paymentId.ToString());
            if (payments != null)
            {
                payments.paymentStatus = paymentDTO.paymentStatus;
                payments.isPayed = paymentDTO.isPayed;
                payments.orderId = paymentDTO.orderId;
                _paymentRepository.Update(payments);
            }
            else
            {
                payments = new Payments();
                payments.paymentStatus = paymentDTO.paymentStatus;
                payments.orderId = paymentDTO.orderId;
                payments.isPayed = paymentDTO.isPayed;
                _paymentRepository.Update(payments);
            }
        }

        public async Task<List<PaymentDto>> PaymentCasso(List<PaymentCasso> paymentCasso)
        {
            if (paymentCasso == null) { return null; }

            List<PaymentDto> paymentDTOs = new List<PaymentDto>();

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
                        paymentDTOs.Add(new PaymentDto
                        {
                            paymentId = payments.id,
                            orderId = payments.orderId,
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
