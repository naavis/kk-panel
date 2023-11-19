namespace KomakallioPanel.JobManagement.Jobs
{
    public class MetsahoviJob : IImageJob
    {
        private readonly IImageDownloader imageDownloader;

        public MetsahoviJob(IImageDownloader imageDownloader)
        {
            this.imageDownloader = imageDownloader;
        }

        public static ImageSettings Settings
            => new("metsahovi",
                   "Metsähovi",
                   new Uri("https://www.metsahovi.fi/allsky-gallery"),
                   new Uri("not-available-image.jpg", UriKind.Relative));

        public async Task ExecuteAsync()
        {
            await imageDownloader.DownloadImageAsync(Settings.Id, new Uri("https://data.metsahovi.fi/allsky/latest_hf.jpeg"));
        }
    }
}
