namespace WebApp.Service.IService
{
    public interface IEmailService 
    {
        Task<bool> SendEmail(string email, string subject, string body);
    }
}
