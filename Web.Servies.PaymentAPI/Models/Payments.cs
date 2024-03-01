using System.ComponentModel.DataAnnotations;
using static Shared.Enums.SD;

namespace Web.Services.PaymentAPI.Models
{
    public class Payments
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public int orderId { get; set; }

        [Required]
        public Boolean isPayed { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus paymentStatus { get; set; }
    }
}
