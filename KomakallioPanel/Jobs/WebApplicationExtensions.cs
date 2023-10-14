using Hangfire;

namespace KomakallioPanel.Jobs
{
    public static class WebApplicationExtensions
    {
        public static void UseRecurringJobs(this WebApplication app)
        {
            var recurringJobs = app.Services.GetRequiredService<IRecurringJobManager>();
            recurringJobs.AddOrUpdate("testJob", () => Console.WriteLine("Hello from a recurring job"), "*/10 * * * * *");
            recurringJobs.Trigger("testJob");
        }
    }
}
