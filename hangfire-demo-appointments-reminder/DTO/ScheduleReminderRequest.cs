namespace hangfire_demo_appointments_reminder.DTO
{
    public record ScheduleReminderRequest(
        string Email,
        string? Subject,
        string? Message,
        DateTimeOffset? DueAtUtc,
        int? DelaySeconds
    );
}
