using Hangfire;
using Hangfire.MemoryStorage;
using hangfire_demo_appointments_reminder.DTO;
using hangfire_demo_appointments_reminder.Jobs;
using hangfire_demo_appointments_reminder.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config =>
{
    config.UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseMemoryStorage();
});
builder.Services.AddHangfireServer();

builder.Services.AddSingleton<INotificationService, EmailNotificationService>();
builder.Services.AddScoped<ReminderDispatcherJob>();


builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate<AppointmentReminderJob>(
    recurringJobId: "daily-demo-reminders",
    methodCall: job => job.RunDemoAsync(),
    cronExpression: "* * * * *"
);

// Program a reminder
app.MapPost("/api/reminders/schedule", (ScheduleReminderRequest req, IBackgroundJobClient jobs) =>
{
    if (req.DueAtUtc is null && req.DelaySeconds is null)
        return Results.BadRequest("Provide either DueAtUtc or DelaySeconds.");

    if (req.DueAtUtc is not null && req.DueAtUtc <= DateTimeOffset.UtcNow)
        return Results.BadRequest("DueAtUtc must be in the future.");

    if (req.DelaySeconds is not null && req.DelaySeconds <= 0)
        return Results.BadRequest("DelaySeconds must be > 0.");

    var subject = req.Subject ?? "Appointment Reminder";
    var body = req.Message ?? "Don't forget your appointment!";

    string jobId;

    if (req.DueAtUtc is not null)
    {
        // Mejor con DateTimeOffset para evitar errores de zona/UTC
        var delay = req.DueAtUtc.Value - DateTimeOffset.UtcNow;
        jobId = jobs.Schedule<ReminderDispatcherJob>(
            j => j.SendAsync(req.Email, subject, body),
            delay
        );
    }
    else
    {
        jobId = jobs.Schedule<ReminderDispatcherJob>(
            j => j.SendAsync(req.Email, subject, body),
            TimeSpan.FromSeconds(req.DelaySeconds!.Value)
        );
    }

    return Results.Ok(new { jobId });
})
.WithName("ScheduleReminder")
.WithOpenApi();

// Upsert job
app.MapPost("/api/reminders/recurring", (CreateRecurringRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Id) || string.IsNullOrWhiteSpace(req.Cron))
        return Results.BadRequest("Id and Cron are required.");

    RecurringJob.AddOrUpdate<AppointmentReminderJob>(req.Id, job => job.RunDemoAsync(), req.Cron);
    return Results.Ok(new { id = req.Id, cron = req.Cron });
})
.WithName("CreateOrUpdateRecurring")
.WithOpenApi();

// Delete job
app.MapDelete("/api/reminders/recurring/{id}", (string id) =>
{
    RecurringJob.RemoveIfExists(id);
    return Results.NoContent();
})
.WithName("DeleteRecurring")
.WithOpenApi();


app.Run();
