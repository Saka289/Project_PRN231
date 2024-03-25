
using Newtonsoft.Json;
using Shared.Enums;
using WebApp.Models.Dtos;
using WebApp.Service.IService;

namespace WebApp.Consumer
{
    public class PaymentConsumer : BackgroundService
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _contextAccessor;

        public PaymentConsumer(IPaymentService paymentService, IHttpContextAccessor contextAccessor, IOrderService orderService, IEmailService emailService)
        {
            _paymentService = paymentService;
            _contextAccessor = contextAccessor;
            _emailService = emailService;
            _orderService = orderService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var session = _contextAccessor.HttpContext.Session;
                string paymentId = session.GetString("paymentId");
                if (paymentId != null)
                {
                    var response = await _paymentService.GetPaymentById(paymentId);
                    if (response != null && response.IsSuccess)
                    {
                        var payment = JsonConvert.DeserializeObject<PaymentDto>(Convert.ToString(response.Result));
                        if (payment.paymentStatus == SD.PaymentStatus.COMPLETED)
                        {
                            await HandlerMessage(payment.orderId);
                            break;
                        }
                    }
                }
            }
        }

        private async Task HandlerMessage(string orderId)
        {
            var response = await _orderService.SearchOrder(orderId);
            if (response != null && response.IsSuccess)
            {
                var order = JsonConvert.DeserializeObject<OrderDto>(Convert.ToString(response.Result));
                await _emailService.SendEmail(order.Email, "Order Success", "");
            }
        }
    }
}
