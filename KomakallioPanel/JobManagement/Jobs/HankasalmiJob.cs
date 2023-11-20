namespace KomakallioPanel.JobManagement.Jobs
{
    public class HankasalmiJob : IImageJob
    {
        private readonly IImageUpdater imageUpdater;

        public HankasalmiJob(IImageUpdater imageUpdater)
        {
            this.imageUpdater = imageUpdater;
        }

        public static ImageSettings Settings
            => new("hankasalmi",
                   "Hankasalmi Allsky",
                   new Uri("http://www.ursa.fi/yhd/sirius/sivut/"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageUpdater.UpdateImageAsync(Settings.Id, new Uri("http://murtoinen.jklsirius.fi/ccd/skywatch/ImageLastFTP_AllSKY.jpg"));
        }
    }
}
