namespace KomakallioPanel.JobManagement.Jobs
{
    public class HankasalmiJob : ImageDownloadJob, IImageJob
    {
        public HankasalmiJob(ILogger<ImageDownloadJob> logger, IHttpClientFactory httpClientFactory, IImageManager imageManager) : base(logger, httpClientFactory, imageManager)
        {
        }

        public static ImageSettings Settings
            => new("hankasalmi",
                   "Hankasalmi Allsky",
                   new Uri("http://www.ursa.fi/yhd/sirius/sivut/"),
                   new Uri("http://murtoinen.jklsirius.fi/ccd/skywatch/ImageLastFTP_AllSKY.jpg"));

        public async Task ExecuteAsync()
        {
            await ExecuteAsync(Settings.Id, Settings.ImageSource, true);
        }
    }
}
