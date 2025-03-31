
namespace KomakallioPanel.JobManagement.Jobs
{
    public class KevolaJob : IImageJob
    {
        private readonly IImageUpdater imageUpdater;

        public KevolaJob(IImageUpdater imageUpdater)
        {
            this.imageUpdater = imageUpdater;
        }

        public static ImageSettings Settings
            => new("kevola",
                   "Kevola",
                   new Uri("https://turunursa.fi/allsky-kevola/"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            await imageUpdater.UpdateImageAsync(Settings.Id, new Uri("https://turunursa.fi/allsky/images/image.jpg"));
        }
    }
}
