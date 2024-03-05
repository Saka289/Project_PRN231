namespace Web.Services.PaymentAPI.Models.Dto
{
    public class PaymentWebHook
    {
        public int error {  get; set; }
        public List<PaymentCasso> data { get; set; }
    }
}
