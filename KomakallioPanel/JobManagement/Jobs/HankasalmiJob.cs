namespace KomakallioPanel.JobManagement.Jobs
{
    public class HankasalmiJob : BaseJob, IImageJob
    {
        public HankasalmiJob(ILogger<BaseJob> logger,
                             IHttpClientFactory httpClientFactory,
                             IImageManager imageManager) : base(Settings.Id, logger, httpClientFactory, imageManager)
        {
        }

        public static ImageSettings Settings
            => new("hankasalmi",
                   "Hankasalmi Allsky",
                   new Uri("http://www.ursa.fi/yhd/sirius/sivut/"),
                   new Uri("http://murtoinen.jklsirius.fi/ccd/skywatch/ImageLastFTP_AllSKY.jpg"));

        public async Task ExecuteAsync()
        {
            await DownloadImageAsync(Settings.ImageSource, true);
        }
    }
}
