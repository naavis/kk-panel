namespace KomakallioPanel.JobManagement.Jobs
{
    public class BromarvJob : ImageDownloadJob, IImageJob
    {
        public BromarvJob(ILogger<ImageDownloadJob> logger, IHttpClientFactory httpClientFactory, IImageManager imageManager) : base(logger, httpClientFactory, imageManager)
        {
        }

        public static ImageSettings Settings
            => new("bromarv",
                   "Bromarv Allsky",
                   new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"),
                   new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"));

        public async Task ExecuteAsync()
        {
            await ExecuteAsync(Settings.Id, Settings.ImageSource, true);
        }
    }
}
