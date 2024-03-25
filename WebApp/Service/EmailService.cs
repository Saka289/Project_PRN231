using MailKit.Net.Smtp;
using MimeKit;
using WebApp.Service.IService;

namespace WebApp.Service
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmail(string email, string subject, string body)
        {
            try
            {
                var emailToSend = new MimeMessage();
                emailToSend.From.Add(new MailboxAddress(subject, "sakanatsuma289@gmail.com"));
                emailToSend.To.Add(MailboxAddress.Parse(email));
                emailToSend.Subject = subject;
                emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = body
                };

                using (var emailClient = new SmtpClient())
                {
                    await emailClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await emailClient.AuthenticateAsync("sakanatsuma289@gmail.com", "icni xdjv imww gshb");
                    await emailClient.SendAsync(emailToSend);
                    await emailClient.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
