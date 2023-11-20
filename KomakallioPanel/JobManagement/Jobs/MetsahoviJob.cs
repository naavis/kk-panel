namespace KomakallioPanel.JobManagement.Jobs
{
    public class MetsahoviJob : IImageJob
    {
        private readonly IImageUpdater imageDownloader;

        public MetsahoviJob(IImageUpdater imageDownloader)
        {
            this.imageDownloader = imageDownloader;
        }

        public static ImageSettings Settings
            => new("metsahovi",
                   "Metsähovi",
                   new Uri("https://www.metsahovi.fi/allsky-gallery"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageDownloader.UpdateImageAsync(Settings.Id, new Uri("https://data.metsahovi.fi/allsky/latest_hf.jpeg"));
        }
    }
}
