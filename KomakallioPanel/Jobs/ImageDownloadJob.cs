namespace KomakallioPanel.Jobs
{
    public class ImageDownloadJob
    {
        private readonly ILogger<ImageDownloadJob> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IImageUpdateNotifier notifier;

        public ImageDownloadJob(ILogger<ImageDownloadJob> logger, IHttpClientFactory httpClientFactory, IImageUpdateNotifier notifier)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.notifier = notifier;
        }

        public async Task ExecuteAsync(string key, Uri imageSource, bool onlyLatest = true)
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

            var timeString = DateTime
                .UtcNow
                .ToString("s", System.Globalization.CultureInfo.InvariantCulture)
                .Replace(":", "-");

            /* Until there is more infrastructure in place to store several images
             * from the same camera and clean them up after some retention period,
             * only saving the latest. */
            var outputFilename = onlyLatest switch
            {
                true => $"{key}-latest.jpg",
                false => $"{key}-{timeString}.jpg"
            };

            await inputImage.SaveAsJpegAsync($"wwwroot/{outputFilename}");

            logger.LogInformation("Finished writing to {filename}", outputFilename);

            notifier.Changed(key, outputFilename);
        }
    }
}
