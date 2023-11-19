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

        public async Task DownloadImageAsync(string jobId, Uri imageSource, bool onlyLatest = true)
        {
            logger.LogInformation("Downloading: {imageSource}", imageSource.ToString());

            using var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(imageSource.AbsoluteUri);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Received {statusCode} when downloading {imageSource}", response.StatusCode, imageSource);
                return;
            }

            using var inputImage = await Image.LoadAsync(await response.Content.ReadAsStreamAsync());
            inputImage.Mutate(i => i.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Min,
                Size = new Size(800, 0),
            }));

            var timeString = DateTime
                .UtcNow
                .ToString("s", System.Globalization.CultureInfo.InvariantCulture)
                .Replace(":", "-");

            /* Until there is more infrastructure in place to store several images
             * from the same camera and clean them up after some retention period,
             * only saving the latest. */
            var outputFilename = onlyLatest switch
            {
                true => $"{jobId}-latest.jpg",
                false => $"{jobId}-{timeString}.jpg"
            };

            await inputImage.SaveAsJpegAsync($"wwwroot/{outputFilename}");

            logger.LogInformation("Finished writing to {filename}", outputFilename);
            var imageRelativeUri = new Uri($"{outputFilename}?cachebuster={Random.Shared.NextInt64()}", UriKind.Relative);
            imageManager.Update(jobId, imageRelativeUri);
        }
    }
}
