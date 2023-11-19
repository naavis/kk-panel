using System.Text.Json;

namespace KomakallioPanel.JobManagement.Jobs
{
    public class Sat24MicrophysicsJob : IImageJob
    {
        public class Layer
        {
            public string Url { get; set; }
        }

        public class JsonResponse
        {
            public IEnumerable<Layer> Layers { get; set; }
        }

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IImageDownloader imageDownloader;

        public Sat24MicrophysicsJob(IHttpClientFactory httpClientFactory, IImageDownloader imageDownloader)
        {
            this.httpClientFactory = httpClientFactory;
            this.imageDownloader = imageDownloader;
        }

        public static ImageSettings Settings
            => new("sat24microphysics",
                   "Sat24 Nightmicrophysics",
                   new Uri("https://www.sat24.com/en-gb/city/667551/hd#selectedLayer=euMicro"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
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

            var lastLayer = parsedJson.Layers.Last();

            var baseUri = new Uri("https://imn-api.meteoplaza.com/v4/nowcast/tiles/", UriKind.Absolute);
            var tilePath = lastLayer.Url switch
            {
                { } when lastLayer.Url.StartsWith("/") => lastLayer.Url.Substring(1),
                _ => lastLayer.Url,
            };
            var fullUri = new Uri(baseUri, tilePath + "/7/27/65/41/82?outputtype=jpeg");

            await imageDownloader.DownloadImageAsync(Settings.Id, fullUri);
        }
    }
}
