using System.Net;
using System.Net.Mail;

namespace WPR_project.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.example.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "noreply@example.com";
        private readonly string _smtpPass = "yourpassword";

        public void SendEmail(string to, string subject, string body)
        {
            var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(to);

            client.Send(mailMessage);
        }
    }
}
