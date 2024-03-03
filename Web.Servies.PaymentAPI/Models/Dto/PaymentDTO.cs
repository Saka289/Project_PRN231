using static Shared.Enums.SD;

namespace Web.Services.PaymentAPI.Models.Dto
{
    public class PaymentDTO
    {
        public string paymentId {  get; set; }
        public bool isPayed { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public decimal refund { get; set; } = 0;
    }
}
