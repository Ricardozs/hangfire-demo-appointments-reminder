namespace hangfire_demo_appointments_reminder.DTO
{
    public record CreateRecurringRequest(string Id, string Cron, string Email, string Subject, string Message);
}
