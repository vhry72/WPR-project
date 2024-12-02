namespace WPR_project.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
}
