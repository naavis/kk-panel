namespace KomakallioPanel.JobManagement.Jobs
{
    public class MetsahoviJob : IImageJob
    {
        private readonly IImageUpdater imageUpdater;

        public MetsahoviJob(IImageUpdater imageUpdater)
        {
            this.imageUpdater = imageUpdater;
        }

        public static ImageSettings Settings
            => new("metsahovi",
                   "Metsähovi",
                   new Uri("https://www.metsahovi.fi/allsky-gallery"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageUpdater.UpdateImageAsync(Settings.Id, new Uri("https://data.metsahovi.fi/allsky/latest_hf.jpeg"));
        }
    }
}
