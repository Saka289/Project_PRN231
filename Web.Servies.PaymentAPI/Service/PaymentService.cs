using AutoMapper;
using Shared.Dtos;
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
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public PaymentService(IPaymentRepository paymentRepository, IUserService userService, IOrderService orderService, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _userService = userService;
            _orderService = orderService;
            _mapper = mapper;
            _response = new ResponseDto();
        }


        public PaymentDto FindById(string id)
        {
            Payments payments = _paymentRepository.FindById(id);

            if (payments != null)
            {
                PaymentDto paymentDto = new PaymentDto
                {
                    id = payments.id,
                    orderId = payments.orderId,
                    isPayed = payments.isPayed,
                    paymentStatus = payments.paymentStatus,
                };
                return paymentDto;
            }
            return null;
        }

        public ResponseDto UpsertPayment(PaymentDto paymentDTO)
        {
            try
            {
                Payments payments = _paymentRepository.FindById(paymentDTO.id.ToString());
                if (paymentDTO.id.Equals(Guid.Empty))
                {
                    Payments obj = _mapper.Map<Payments>(paymentDTO);
                    var response = _paymentRepository.Create(obj);
                    _paymentRepository.Save();
                    _response.Result = _mapper.Map<PaymentDto>(response);
                    _response.Message = "Payment created Successfully !!!";
                }
                else
                {
                    Payments obj = _mapper.Map<Payments>(paymentDTO);
                    _paymentRepository.Update(obj);
                    _paymentRepository.Save();
                    _response.Result = _mapper.Map<PaymentDto>(obj);
                    _response.Message = "Payment updated Successfully !!!";
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
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
                    if (destrip[j].Contains("ECOMMERCE"))
                    {
                        indexIdOrder = j;
                    }
                }
                string OrderId = destrip[indexIdOrder + 1];

                OrderDto order = await _orderService.GetOrder(OrderId);
                Payments payments = _paymentRepository.FindByOrderId(OrderId);
                if (payments != null && order != null)
                {
                    if (payment.Amount == order.OrderTotal)
                    {
                        payments.paymentStatus = PaymentStatus.COMPLETED;
                    }
                    else if (payment.Amount < order.OrderTotal)
                    {
                        payments.refund = payment.Amount;
                        payments.paymentStatus = PaymentStatus.REFUND;
                    }
                    else if (payment.Amount > order.OrderTotal)
                    {
                        decimal refund = payment.Amount - order.OrderTotal;
                        payments.refund = refund;
                        payments.paymentStatus = PaymentStatus.COMPLETED;
                    }
                    _paymentRepository.Update(payments);
                    paymentDTOs.Add(new PaymentDto
                    {
                        id = payments.id,
                        orderId = payments.orderId,
                        paymentStatus = payments.paymentStatus,
                        refund = payments.refund,
                    });
                    _paymentRepository.Save();
                }
            }
            return paymentDTOs;
        }

        public async Task<List<PaymentDto>> GetPayments()
        {
            return _paymentRepository.FindAll().Select(ite => new PaymentDto
            {
                id=ite.id,
                orderId=ite.orderId,
                paymentStatus=ite.paymentStatus,
                refund=ite.refund,
            }).ToList();
        }
    }
}
