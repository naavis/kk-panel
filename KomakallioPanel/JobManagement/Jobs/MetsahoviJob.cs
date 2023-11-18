namespace KomakallioPanel.JobManagement.Jobs
{
    public class MetsahoviJob : ImageDownloadJob, IImageJob
    {
        public MetsahoviJob(ILogger<ImageDownloadJob> logger,
                            IHttpClientFactory httpClientFactory,
                            IImageManager imageManager) : base(logger, httpClientFactory, imageManager)
        {
        }

        public static ImageSettings Settings
            => new("metsahovi",
                   "Metsähovi",
                   new Uri("https://www.metsahovi.fi/allsky-gallery"),
                   new Uri("https://data.metsahovi.fi/allsky/latest_hf.jpeg"));

        public async Task ExecuteAsync()
        {
            await ExecuteAsync(Settings.Id, Settings.ImageSource, true);
        }
    }
}
