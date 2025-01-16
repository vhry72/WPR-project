namespace WPR_project.Services.Email
{
    public interface IEmailService
    {
        void SendEmailWithAttachment(string naarGebruiker, string subject, string body, byte[] attachmentData, string attachmentName);
        void SendEmail(string naarGebruiker, string subject, string body);
    }
}
