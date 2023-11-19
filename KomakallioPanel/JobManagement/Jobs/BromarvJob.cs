namespace KomakallioPanel.JobManagement.Jobs
{
    public class BromarvJob : BaseJob, IImageJob
    {
        public BromarvJob(ILogger<BromarvJob> logger,
                          IHttpClientFactory httpClientFactory,
                          IImageManager imageManager) : base(Settings.Id, logger, httpClientFactory, imageManager)
        {
        }

        public static ImageSettings Settings
            => new("bromarv",
                   "Bromarv Allsky",
                   new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"),
                   new Uri("not-available-image.jpg", UriKind.Relative));

        public async Task ExecuteAsync()
        {
            await DownloadImageAsync(new Uri("https://bromarv-astro.cloud/allsky-latest.jpg", UriKind.Absolute), true);
        }
    }
}
