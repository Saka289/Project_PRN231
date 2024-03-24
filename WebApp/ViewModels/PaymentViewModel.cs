using WebApp.Models.Dtos;

namespace WebApp.ViewModels
{
    public class PaymentViewModel
    {
        public OrderDto OrderDto { get; set; }
        public string ImageQR { get; set; }
    }
}
