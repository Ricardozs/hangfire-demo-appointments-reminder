using hangfire_demo_appointments_reminder.Services;

namespace hangfire_demo_appointments_reminder.Jobs
{
    public class AppointmentReminderJob(INotificationService notifier, ILogger<AppointmentReminderJob> logger)
    {
        private readonly INotificationService _notifier = notifier;
        private readonly ILogger<AppointmentReminderJob> _logger = logger;

        public async Task RunDemoAsync(string email, string subject, string message)
        {
            var now = DateTimeOffset.UtcNow;
            _logger.LogInformation("📩 [AppointmentReminderJob] Sending reminder to {Email} at {Time}", email, now);
            await _notifier.SendEmailAsync(email, subject, message);
        }
    }
}
