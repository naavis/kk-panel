namespace KomakallioPanel.ImageTools
{
    public class ImageDownloader : IImageDownloader
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ImageDownloader> logger;

        public ImageDownloader(IHttpClientFactory httpClientFactory,
                               ILogger<ImageDownloader> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<Image> DownloadImageAsync(Uri imageUrl)
        {
            using var httpClient = httpClientFactory.CreateClient();

            logger.LogInformation("Downloading: {imageSource}", imageUrl.ToString());
            var response = await httpClient.GetAsync(imageUrl.AbsoluteUri);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Received status code '{response.StatusCode}' when downloading {imageUrl}");
            }

            var image = await Image.LoadAsync(await response.Content.ReadAsStreamAsync());
            if (image is null)
            {
                throw new InvalidOperationException($"Could not parse response from {imageUrl} to image");
            }

            logger.LogInformation("Finished downloading image from {imageSource}", imageUrl);
            return image;
        }
    }
}
