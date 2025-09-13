using hangfire_demo_appointments_reminder.Services;

namespace hangfire_demo_appointments_reminder.Jobs
{
    public class AppointmentReminderJob(INotificationService notifier, ILogger<AppointmentReminderJob> logger)
    {
        private readonly INotificationService _notifier = notifier;
        private readonly ILogger<AppointmentReminderJob> _logger = logger;

        public async Task RunDemoAsync()
        {
            var now = DateTimeOffset.UtcNow;
            _logger.LogInformation("Running demo reminder job at {Time}", now);
            await _notifier.SendEmailAsync("demo@example.com", "Daily Reminder", $"Hello! This is your daily reminder at {now:u}");
        }
    }
}
