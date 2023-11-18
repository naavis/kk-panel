namespace KomakallioPanel.JobManagement.Jobs
{
    public class BromarvJob : BaseJob, IImageJob
    {
        public BromarvJob(ILogger<BaseJob> logger,
                          IHttpClientFactory httpClientFactory,
                          IImageManager imageManager) : base(Settings.Id, logger, httpClientFactory, imageManager)
        {
        }

        public static ImageSettings Settings
            => new("bromarv",
                   "Bromarv Allsky",
                   new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"),
                   new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"));

        public async Task ExecuteAsync()
        {
            await DownloadImageAsync(Settings.ImageSource, true);
        }
    }
}
