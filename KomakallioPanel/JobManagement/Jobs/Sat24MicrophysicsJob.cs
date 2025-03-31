using KomakallioPanel.ImageTools;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;

namespace KomakallioPanel.JobManagement.Jobs
{
    public class Sat24MicrophysicsJob : IImageJob
    {
        private class Layer
        {
            public string Url { get; set; } = default!;
        }

        private class JsonResponse
        {
            public IEnumerable<Layer> Layers { get; set; } = default!;
        }

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IImageUpdater imageUpdater;
        private readonly IImageDownloader imageDownloader;

        public Sat24MicrophysicsJob(IHttpClientFactory httpClientFactory,
                                    IImageUpdater imageUpdater,
                                    IImageDownloader imageDownloader)
        {
            this.httpClientFactory = httpClientFactory;
            this.imageUpdater = imageUpdater;
            this.imageDownloader = imageDownloader;
        }

        public static ImageSettings Settings
            => new("sat24microphysics",
                   "Sat24 Nightmicrophysics",
                   new Uri("https://www.sat24.com/en-gb/city/667551/hd#selectedLayer=euMicro"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            JsonResponse? parsedJson = await GetLayerListAsync();
            var lastLayer = parsedJson.Layers.Last();
            var satelliteUrl = GetFullImageUrl(lastLayer);
            using var satelliteImage = await imageDownloader.DownloadImageAsync(satelliteUrl);

            const string countryBordersUrl = "https://maptiler.infoplaza.io/api/maps/Border/static/25.48,64.56,4.2/1176x882.png?attribution=false";
            using var countryBordersImage = await imageDownloader.DownloadImageAsync(new Uri(countryBordersUrl, UriKind.Absolute));

            // Align satellite image to country borders image
            satelliteImage.Mutate(image =>
                image.Crop(new Rectangle(15, 215, 4095, 3019))
                     .Resize(countryBordersImage.Size));

            // Add country borders to satellite image and crop excess
            using var outputImage = countryBordersImage.Clone(ctx =>
                ctx.DrawImage(satelliteImage, PixelColorBlendingMode.Add, 1.0f)
                   .Crop(new Rectangle(200, 200, 800, 600)));

            await imageUpdater.UpdateImageAsync(Settings.Id, outputImage);
        }

        private static Uri GetFullImageUrl(Layer layer)
        {
            var baseUri = new Uri("https://imn-api.meteoplaza.com/v4/nowcast/tiles/", UriKind.Absolute);
            /* If layer.Url starts with a slash, cambining it with the baseUri using the
             * Uri constructor erases the path in the baseUri, resulting in the wrong URL */
            var tilePath = layer.Url.StartsWith("/") ? layer.Url[1..] : layer.Url;
            var fullUri = new Uri(baseUri, tilePath + "/7/27/65/41/82?outputtype=jpeg");
            return fullUri;
        }

        private async Task<JsonResponse> GetLayerListAsync()
        {
            var httpClient = httpClientFactory.CreateClient();
            var jsonResult = await httpClient.GetAsync("https://imn-api.meteoplaza.com/v4/nowcast/tiles/satellite-europe-nightmicrophysics/");

            var parsedJson = await JsonSerializer.DeserializeAsync<JsonResponse>(
                await jsonResult.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            if (parsedJson is null)
            {
                throw new InvalidOperationException("Could not parse JSON from Sat24");
            }

            return parsedJson;
        }
    }
}
