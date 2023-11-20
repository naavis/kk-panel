namespace KomakallioPanel.JobManagement.Jobs
{
    public class HankasalmiJob : IImageJob
    {
        private readonly IImageUpdater imageDownloader;

        public HankasalmiJob(IImageUpdater imageDownloader)
        {
            this.imageDownloader = imageDownloader;
        }

        public static ImageSettings Settings
            => new("hankasalmi",
                   "Hankasalmi Allsky",
                   new Uri("http://www.ursa.fi/yhd/sirius/sivut/"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageDownloader.UpdateImageAsync(Settings.Id, new Uri("http://murtoinen.jklsirius.fi/ccd/skywatch/ImageLastFTP_AllSKY.jpg"));
        }
    }
}
