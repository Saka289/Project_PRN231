using System.ComponentModel.DataAnnotations;

namespace Web.Services.OrderAPI.Models.Dto
{
    public class RequestQrDto
    {
        public string accountNo { get; set; }
        public string accountName { get; set; }
        public string acqId { get; set; }
        public string addInfo { get; set; }
        [Required]
        public decimal amount { get; set; }
        public string template { get; set; }
    }
}
