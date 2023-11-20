namespace KomakallioPanel.JobManagement.Jobs
{
    public class TampereSouthJob : IImageJob
    {
        private readonly IImageUpdater imageUpdater;

        public TampereSouthJob(IImageUpdater imageUpdater)
        {
            this.imageUpdater = imageUpdater;
        }

        public static ImageSettings Settings
            => new("tamperesouth",
                   "Tampere South",
                   new Uri("https://www.ursa.fi/yhd/tampereenursa/"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageUpdater.UpdateImageAsync(Settings.Id, new Uri("https://www.ursa.fi/yhd/tampereenursa/ys-images/south-snapshot.jpg"));
        }
    }
}
