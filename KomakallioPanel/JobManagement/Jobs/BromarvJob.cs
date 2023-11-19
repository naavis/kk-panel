namespace KomakallioPanel.JobManagement.Jobs
{
    public class BromarvJob : IImageJob
    {
        private readonly IImageDownloader imageDownloader;

        public BromarvJob(IImageDownloader imageDownloader)
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
            await imageDownloader.DownloadImageAsync(Settings.Id, new Uri("https://bromarv-astro.cloud/allsky-latest.jpg"));
        }
    }
}
