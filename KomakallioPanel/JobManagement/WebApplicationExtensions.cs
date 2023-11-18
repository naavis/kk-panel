using KomakallioPanel.JobManagement.Jobs;

namespace KomakallioPanel.JobManagement
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureJobs(this WebApplication app)
        {
            var imageManager = app.Services.GetRequiredService<IImageManager>();
            imageManager.Add<MetsahoviJob>("30 */3 * * * *");
            imageManager.Add<BromarvJob>("30 * * * * *");
            imageManager.Add<HankasalmiJob>("30 * * * * *");
        }
    }
}
