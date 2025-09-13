# 📌 Hangfire Appointment Reminders Demo

A minimal **.NET 9 Web API** demo showcasing how to schedule **background jobs** and **recurring jobs** with [Hangfire](https://www.hangfire.io/).  

This project demonstrates:  
- **Minimal APIs** in .NET 9  
- **Hangfire integration** with ASP.NET Core  
- **In-memory storage** for simplicity (no external DB required)  
- **Job scheduling** (one-time jobs at a specific time or delay)  
- **Recurring jobs** using CRON expressions  
- **Hangfire Dashboard** for monitoring jobs  

---

## 🛠 Tech Stack
- **.NET 9**  
- **ASP.NET Core Minimal API**  
- **Hangfire.AspNetCore**  
- **Hangfire.MemoryStorage** (for demo purposes)  

---

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/<your-username>/hangfire-appointment-reminders.git
cd hangfire-appointment-reminders
```

### 2. Run the project
```bash
dotnet restore
dotnet run --project hangfire_demo_appointments_reminder
```

### 3. Access the services
- **Hangfire Dashboard** → [http://localhost:5138/hangfire](http://localhost:5138/hangfire)  

> ⚠️ The project is configured to run **only on HTTP** (`http://localhost:5138`).  
> No HTTPS profile is included to avoid issues with self-signed certificates and to keep curl examples consistent.  

---

## 📬 API Endpoints

### 🔹 1. Schedule a one-time reminder
Schedules a reminder email either **after X seconds** or **at a specific UTC date/time**.

```bash
curl --location 'http://localhost:5138/api/reminders/schedule' --header 'Content-Type: application/json' --data-raw '{
  "email": "test@example.com",
  "subject": "Demo",
  "message": "This is a test reminder for the demo!",
  "delaySeconds": 5
}'
```

✅ Expected: After 5 seconds, the log will show a demo email reminder being “sent”.

---

### 🔹 2. Create or update a recurring job
Adds or updates a job that runs periodically, defined by a CRON expression.

```bash
curl --location 'http://localhost:5138/api/reminders/recurring' --header 'Content-Type: application/json' --data-raw '{
  "id": "test-job",
  "cron": "* * * * *",
  "email": "demo-test2@test.com",
  "subject": "Recurrent job 2",
  "message": "This is a new job!"
}'
```

- `"* * * * *"` → run every minute  
- `"0 9 * * *"` → run every day at 09:00 UTC  
- `"*/5 * * * *"` → run every 5 minutes  

✅ Expected: The job will run every minute and you’ll see logs indicating the reminder email was “sent”.

---

### 🔹 3. Delete a recurring job
Removes the recurring job by its ID.

```bash
curl --location --request DELETE 'http://localhost:5138/api/reminders/recurring/test-job'
```

✅ Expected: The job no longer appears in the Hangfire dashboard and stops executing.  

---

## 📊 Monitoring Jobs
Hangfire provides a dashboard at [http://localhost:5138/hangfire](http://localhost:5138/hangfire) where you can:  
- See **scheduled jobs**  
- Inspect **recurring jobs**  
- Manually **trigger or retry jobs**  
- Check job history and logs  

---

## ⚠️ Notes
- This demo uses **Hangfire.MemoryStorage** for simplicity.  
  - For production, replace it with a persistent storage (SQL Server, PostgreSQL, Redis, etc.).  
- The **EmailNotificationService** is mocked → it just logs output to the console.  
  - Replace it with a real provider (SendGrid, SES, Twilio, etc.) in a real-world scenario.  
- The Hangfire dashboard (`/hangfire`) is unprotected in this demo.  
  - In production, always secure it with authentication or IP restrictions.  

---

## ✅ What This Demo Shows
- How to build a minimal API with .NET 9.  
- How to integrate Hangfire for **background job processing**.  
- How to handle **one-time jobs** and **recurring jobs**.  
- How to expose endpoints that schedule jobs dynamically.  
- How to monitor jobs via Hangfire dashboard.  
