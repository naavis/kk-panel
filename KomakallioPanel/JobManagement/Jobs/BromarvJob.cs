namespace KomakallioPanel.JobManagement.Jobs
{
    public class BromarvJob : IImageJob
    {
        private readonly IImageUpdater imageDownloader;

        public BromarvJob(IImageUpdater imageDownloader)
        {
            this.imageDownloader = imageDownloader;
        }

        public static ImageSettings Settings
            => new("bromarv",
                   "Bromarv Allsky",
                   new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageDownloader.UpdateImageAsync(Settings.Id, new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"));
        }
    }
}
