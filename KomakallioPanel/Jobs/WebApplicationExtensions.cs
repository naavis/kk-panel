using Hangfire;

namespace KomakallioPanel.Jobs
{
    public static class WebApplicationExtensions
    {
        public static void UseRecurringJobs(this WebApplication app)
        {
            var recurringJobs = app.Services.GetRequiredService<IRecurringJobManager>();
            AddJob(recurringJobs, "metsahovi", new Uri("https://data.metsahovi.fi/allsky/latest_hf.jpeg"), "30 */3 * * * *");
        }

        private static void AddJob(IRecurringJobManager recurringJobs, string key, Uri imageSource, string cronSchedule)
        {
            recurringJobs.AddOrUpdate<ImageDownloadJob>(key,
                                                        job => job.ExecuteAsync(key, imageSource, true),
                                                        cronSchedule);
            recurringJobs.Trigger(key);
        }
    }
}
