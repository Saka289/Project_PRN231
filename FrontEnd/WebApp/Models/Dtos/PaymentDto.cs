using static Shared.Enums.SD;

namespace WebApp.Models.Dtos
{
    public class PaymentDto
    {
        public Guid id { get; set; }
        public string orderId { get; set; }
        public bool isPayed { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public decimal refund { get; set; } = 0;
    }
}
