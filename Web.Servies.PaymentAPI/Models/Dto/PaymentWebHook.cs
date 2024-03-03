namespace Web.Services.PaymentAPI.Models.Dto
{
    public class PaymentWebHook
    {
        public string error {  get; set; }
        public List<PaymentCasso> data { get; set; }
    }
}
