namespace WPR_project.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(string naarGebruiker, string subject, string body);
    }
}
