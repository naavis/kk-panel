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

        public Sat24MicrophysicsJob(IHttpClientFactory httpClientFactory, IImageUpdater imageUpdater)
        {
            this.httpClientFactory = httpClientFactory;
            this.imageUpdater = imageUpdater;
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
            var fullUri = GetFullImageUrl(lastLayer);
            await imageUpdater.UpdateImageAsync(Settings.Id, fullUri);

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
