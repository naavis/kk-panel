namespace KomakallioPanel.JobManagement.Jobs
{
    public class TahtikallioJob : IImageJob
    {
        private readonly IImageUpdater imageUpdater;

        public TahtikallioJob(IImageUpdater imageUpdater)
        {
            this.imageUpdater = imageUpdater;
        }

        public static ImageSettings Settings
            => new("tahtikallio",
                   "Tähtikallio",
                   new Uri("https://www.ursa.fi/tahtikallio.html"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageUpdater.UpdateImageAsync(Settings.Id, new Uri("https://www.ursa.fi/fileadmin/ursa2010/kuvat/tahtikallio/tahtikallio-allsky.jpg"));
        }
    }
}
