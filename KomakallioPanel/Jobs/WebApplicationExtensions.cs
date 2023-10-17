using Hangfire;

namespace KomakallioPanel.Jobs
{
    public static class WebApplicationExtensions
    {
        public static void UseRecurringJobs(this WebApplication app)
        {
            var recurringJobs = app.Services.GetRequiredService<IRecurringJobManager>();
            recurringJobs.AddOrUpdate<ImageDownloadJob>("metsahovi",
                                                        job => job.ExecuteAsync("metsahovi", new Uri("https://data.metsahovi.fi/allsky/latest_hf.jpeg"), true),
                                                        "30 */3 * * * *");
            recurringJobs.Trigger("metsahovi");
        }
    }
}
