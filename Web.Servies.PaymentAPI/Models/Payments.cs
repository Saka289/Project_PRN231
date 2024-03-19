using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Shared.Enums.SD;

namespace Web.Services.PaymentAPI.Models
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public string orderId { get; set; }

        [Required]
        public bool isPayed { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus paymentStatus { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal refund { get; set; }
    }
}
