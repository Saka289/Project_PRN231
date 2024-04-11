namespace Web.Services.PaymentAPI.Models.Dto
{
    public class PaymentCasso
    {
        public int Id { get; set; }
        public string When { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public int CusumBalance { get; set; }
        public string? Tid { get; set; }
        public string? SubAccountId { get; set; }
        public string? BankSubAccountId { get; set; }
        public string? VirtualAccount { get; set; }
        public string? VirtualAccountName { get; set; }
        public string? CorresponsiveName { get; set; }
        public string? CorresponsiveAccount { get; set; }
        public string? CorresponsiveBankId { get; set; }
        public string? CorresponsiveBankName { get; set; }
    }
}
