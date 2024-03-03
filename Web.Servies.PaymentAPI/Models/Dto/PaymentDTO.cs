using static Shared.Enums.SD;

namespace Web.Services.PaymentAPI.Models.Dto
{
    public class PaymentDto
    {
        public Guid paymentId {  get; set; }
        public Guid orderId { get; set; }
        public bool isPayed { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public decimal refund { get; set; } = 0;
    }
}
