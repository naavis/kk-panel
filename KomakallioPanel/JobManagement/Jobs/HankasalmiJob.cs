namespace KomakallioPanel.JobManagement.Jobs
{
    public class HankasalmiJob : BaseJob, IImageJob
    {
        public HankasalmiJob(ILogger<HankasalmiJob> logger,
                             IHttpClientFactory httpClientFactory,
                             IImageManager imageManager) : base(Settings.Id, logger, httpClientFactory, imageManager)
        {
        }

        public static ImageSettings Settings
            => new("hankasalmi",
                   "Hankasalmi Allsky",
                   new Uri("http://www.ursa.fi/yhd/sirius/sivut/"),
                   new Uri("not-available-image.jpg", UriKind.Relative));

        public async Task ExecuteAsync()
        {
            await DownloadImageAsync(new Uri("http://murtoinen.jklsirius.fi/ccd/skywatch/ImageLastFTP_AllSKY.jpg", UriKind.Absolute), true);
        }
    }
}
