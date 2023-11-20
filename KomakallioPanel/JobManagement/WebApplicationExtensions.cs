using KomakallioPanel.JobManagement.Jobs;

namespace KomakallioPanel.JobManagement
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureJobs(this WebApplication app)
        {
            var imageManager = app.Services.GetRequiredService<IImageManager>();
            imageManager.Add<MetsahoviJob>("30 */3 * * * *");
            imageManager.Add<FmiTestbedJob>("0 */5 * * * *");
            imageManager.Add<BromarvJob>("30 * * * * *");
            imageManager.Add<Sat24MicrophysicsJob>("30 */5 * * * *");
            imageManager.Add<TampereNorthJob>("30 * * * * *");
            imageManager.Add<TampereSouthJob>("30 * * * * *");
            imageManager.Add<HankasalmiJob>("30 * * * * *");
        }
    }
}
