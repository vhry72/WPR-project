namespace WPR_project.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(string naar, string subject, string body);
    }
}
