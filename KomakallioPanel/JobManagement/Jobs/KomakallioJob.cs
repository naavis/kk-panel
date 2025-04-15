namespace KomakallioPanel.JobManagement.Jobs
{
    public class KomakallioJob : IImageJob
    {
        private readonly IImageUpdater imageUpdater;

        public KomakallioJob(IImageUpdater imageUpdater)
        {
            this.imageUpdater = imageUpdater;
        }

        public static ImageSettings Settings
            => new("komakallio",
                   "Komakallio",
                   new Uri("https://taivas.komakallio.fi"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageUpdater.UpdateImageAsync(Settings.Id, new Uri("https://taivas.komakallio.fi/images/latest.jpg"));
        }
    }
}
