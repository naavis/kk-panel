using KomakallioPanel.ImageTools;

namespace KomakallioPanel.JobManagement
{
    public class ImageUpdater : IImageUpdater
    {
        private readonly ILogger<ImageUpdater> logger;
        private readonly IImageDownloader imageDownloader;
        private readonly IImageManager imageManager;

        public ImageUpdater(ILogger<ImageUpdater> logger,
                            IImageDownloader imageDownloader,
                            IImageManager imageManager)
        {
            this.logger = logger;
            this.imageDownloader = imageDownloader;
            this.imageManager = imageManager;
        }

        public async Task UpdateImageAsync(string jobId, Uri imageSource)
        {
            using var downloadedImage = await imageDownloader.DownloadImageAsync(imageSource);
            await UpdateImageAsync(jobId, downloadedImage);
        }

        public async Task UpdateImageAsync(string jobId, Image image)
        {
            ResizeImageToWidth(image, 800);
            await SaveImage(jobId, image);
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

        private async Task SaveImage(string jobId, Image inputImage)
        {
            var filename = $"{jobId}-latest.jpg";
            await inputImage.SaveAsJpegAsync($"wwwroot/{filename}");
            logger.LogInformation("Finished writing to {filename}", filename);
            NotifyAboutUpdate(jobId, filename);
        }

        private void NotifyAboutUpdate(string jobId, string outputFilename)
        {
            var imageRelativeUri = new Uri($"{outputFilename}?cachebuster={Random.Shared.NextInt64()}", UriKind.Relative);
            imageManager.Update(jobId, imageRelativeUri);
        }
    }
}
