using System.Net;
using System.Net.Mail;

namespace WPR_project.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailService(IConfiguration configuration)
        {
            // Haal waarden op uit appsettings.json
            _smtpServer = configuration["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
            _smtpUser = configuration["EmailSettings:SmtpUser"];
            _smtpPass = configuration["EmailSettings:SmtpPass"];
            _senderEmail = configuration["EmailSettings:SenderEmail"];
            _senderName = configuration["EmailSettings:SenderName"];
        }

        public void SendEmailWithAttachment(string naarGebruiker, string subject, string body, byte[] attachmentData, string attachmentName)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            })
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail, _senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(naarGebruiker);

                // Voeg de bijlage toe
                var stream = new MemoryStream(attachmentData);
                var attachment = new Attachment(stream, attachmentName, "application/pdf");
                mailMessage.Attachments.Add(attachment);

                client.Send(mailMessage);

                // Zorg ervoor dat de stream en mailMessage correct worden opgeruimd
                stream.Dispose();
                mailMessage.Dispose();
            }
        }

        public async Task SendEmailWithImage(string naarGebruiker, string subject, string body, byte[] attachmentData)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            })
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail, _senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(naarGebruiker);

                using (var stream = new MemoryStream(attachmentData))
                {
                    var attachment = new Attachment(stream, "image.png", "image/png");
                    mailMessage.Attachments.Add(attachment);

                    try
                    {

                        await client.SendMailAsync(mailMessage);
                    }
                    catch (Exception ex)
                    {

                        throw new InvalidOperationException("Error sending email with image attachment.", ex);
                    }
                    finally
                    {

                        mailMessage.Attachments.Dispose();
                        attachment.Dispose();
                    }
                }
            }
        }


        public void SendEmail(string naarGebruiker, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true // Gebruik SSL/TLS voor een veilige verbinding
            })
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail, _senderName), // Gebruik herkenbare afzender
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // Zet op true als je HTML-e-mails verstuurt
                };
                mailMessage.To.Add(naarGebruiker);

                client.Send(mailMessage); // Verstuur de e-mail
            }
        }

        public async Task SendEmailAsync(string naarGebruiker, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true // Gebruik SSL/TLS voor een veilige verbinding
            })
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail, _senderName), // Gebruik herkenbare afzender
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // Zet op true als je HTML-e-mails verstuurt
                };
                mailMessage.To.Add(naarGebruiker);

                await client.SendMailAsync(mailMessage); // Asynchroon versturen van de e-mail
            }
        }
    }
}