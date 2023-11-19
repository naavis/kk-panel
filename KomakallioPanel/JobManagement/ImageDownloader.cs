namespace KomakallioPanel.JobManagement
{
    public class ImageDownloader : IImageDownloader
    {
        private readonly ILogger<ImageDownloader> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IImageManager imageManager;

        public ImageDownloader(ILogger<ImageDownloader> logger,
                          IHttpClientFactory httpClientFactory,
                          IImageManager imageManager)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.imageManager = imageManager;
        }

        public async Task DownloadImageAsync(string jobId, Uri imageSource)
        {
            using var inputImage = await FetchImageAsync(imageSource);
            ResizeImageToWidth(inputImage, 800);

            var outputFilename = $"{jobId}-latest.jpg";
            await inputImage.SaveAsJpegAsync($"wwwroot/{outputFilename}");
            logger.LogInformation("Finished writing to {filename}", outputFilename);

            NotifyAboutUpdate(jobId, outputFilename);
        }

        private void NotifyAboutUpdate(string jobId, string outputFilename)
        {
            var imageRelativeUri = new Uri($"{outputFilename}?cachebuster={Random.Shared.NextInt64()}", UriKind.Relative);
            imageManager.Update(jobId, imageRelativeUri);
        }

        private static void ResizeImageToWidth(Image inputImage, int Width)
        {
            inputImage.Mutate(i =>
            {
                i.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Min,
                    Size = new Size(Width, 0),
                });
            });
        }

        private async Task<Image> FetchImageAsync(Uri imageSource)
        {
            logger.LogInformation("Downloading: {imageSource}", imageSource.ToString());
            using var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(imageSource.AbsoluteUri);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Received {response.StatusCode} when downloading {imageSource}");
            }

            var image = await Image.LoadAsync(await response.Content.ReadAsStreamAsync());
            if (image is null)
            {
                throw new InvalidOperationException($"Could not parse response from {imageSource} to image");
            }

            logger.LogInformation("Finished downloading image from {imageSource}", imageSource);
            return image;
        }
    }
}
