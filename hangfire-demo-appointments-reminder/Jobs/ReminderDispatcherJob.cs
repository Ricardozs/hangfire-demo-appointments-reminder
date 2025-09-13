using hangfire_demo_appointments_reminder.Services;

namespace hangfire_demo_appointments_reminder.Jobs
{
    public class ReminderDispatcherJob(INotificationService notifier, ILogger<ReminderDispatcherJob> logger)
    {
        private readonly INotificationService _notifier = notifier;
        private readonly ILogger<ReminderDispatcherJob> _logger = logger;

        public async Task SendAsync(string email, string subject, string body)
        {
            _logger.LogInformation("Sending reminder to {Email}", email);
            await _notifier.SendEmailAsync(email, subject, body);
        }
    }
}
