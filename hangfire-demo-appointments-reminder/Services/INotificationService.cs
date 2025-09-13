namespace hangfire_demo_appointments_reminder.Services
{
    public interface INotificationService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
