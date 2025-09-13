namespace hangfire_demo_appointments_reminder.Services
{
    public class EmailNotificationService(ILogger<EmailNotificationService> logger) : INotificationService
    {
        private readonly ILogger<EmailNotificationService> _logger = logger;

        public Task SendEmailAsync(string to, string subject, string body)
        {
            _logger.LogInformation("[DEMO EMAIL] To: {To} | Subject: {Subject} | Body: {Body}", to, subject, body);
            return Task.CompletedTask;
        }
    }
}
