namespace KomakallioPanel.JobManagement.Jobs
{
    public class MetsahoviJob : BaseJob, IImageJob
    {
        public MetsahoviJob(ILogger<BaseJob> logger,
                            IHttpClientFactory httpClientFactory,
                            IImageManager imageManager) : base(Settings.Id, logger, httpClientFactory, imageManager)
        {
        }

        public static ImageSettings Settings
            => new("metsahovi",
                   "Metsähovi",
                   new Uri("https://www.metsahovi.fi/allsky-gallery"),
                   new Uri("https://data.metsahovi.fi/allsky/latest_hf.jpeg"));

        public async Task ExecuteAsync()
        {
            await DownloadImageAsync(Settings.ImageSource, true);
        }
    }
}
