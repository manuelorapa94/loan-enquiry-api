using System.Net;
using System.Net.Mail;

namespace LoanEnquiryApi.Service
{
    public class EmailService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public void Send(string email, string subject, string content)
        {
            try
            {
                var host = _configuration["Email:Host"];
                int.TryParse(_configuration["Email:Port"], out var port);
                var username = _configuration["Email:Username"];
                var password = _configuration["Email:Password"];
                var senderEmail = _configuration["Email:SenderEmail"];

                using var smtpClient = new SmtpClient(host)
                {
                    Port = port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail!),
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
            }
            catch (Exception) { }
        }
    }
}
