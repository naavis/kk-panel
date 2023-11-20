namespace KomakallioPanel.JobManagement.Jobs
{
    public class TampereNorthJob : IImageJob
    {
        private readonly IImageUpdater imageUpdater;

        public TampereNorthJob(IImageUpdater imageUpdater)
        {
            this.imageUpdater = imageUpdater;
        }

        public static ImageSettings Settings
            => new("tamperenorth",
                   "Tampere North",
                   new Uri("https://www.ursa.fi/yhd/tampereenursa/"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageUpdater.UpdateImageAsync(Settings.Id, new Uri("https://www.ursa.fi/yhd/tampereenursa/ys-images/north-snapshot.jpg"));
        }
    }
}
