namespace KomakallioPanel.JobManagement.Jobs
{
    public class BromarvJob : IImageJob
    {
        private readonly IImageUpdater imageUpdater;

        public BromarvJob(IImageUpdater imageUpdater)
        {
            this.imageUpdater = imageUpdater;
        }

        public static ImageSettings Settings
            => new("bromarv",
                   "Bromarv Allsky",
                   new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageUpdater.UpdateImageAsync(Settings.Id, new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"));
        }
    }
}
