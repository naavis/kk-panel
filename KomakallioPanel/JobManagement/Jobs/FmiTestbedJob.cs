using AngleSharp;

namespace KomakallioPanel.JobManagement.Jobs
{
    public class FmiTestbedJob : IImageJob
    {
        private readonly IImageUpdater imageDownloader;

        public FmiTestbedJob(IImageUpdater imageDownloader)
        {
            this.imageDownloader = imageDownloader;
        }

        public static ImageSettings Settings
            => new("testbed",
                   "FMI Testbed",
                   new Uri("https://testbed.fmi.fi/"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            string imageUrl = await ParseImageUrl();
            await imageDownloader.UpdateImageAsync(Settings.Id, new Uri(imageUrl));
        }

        private static async Task<string> ParseImageUrl()
        {
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var address = "https://testbed.fmi.fi/?imgtype=radar&t=5&n=1";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);

            var imageTag = document.QuerySelector("img[id=\"anim_image_anim_anim\"]");
            var imageUrl = imageTag?.Attributes["src"]?.Value;

            if (imageUrl is null)
            {
                throw new InvalidOperationException("Could not find URL for FMI testbed");
            }

            return imageUrl;
        }
    }
}
