using AngleSharp;
using KomakallioPanel.ImageTools;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace KomakallioPanel.JobManagement.Jobs
{
    public class Sat24MicrophysicsJob(
        IImageUpdater imageUpdater,
        IImageDownloader imageDownloader) : IImageJob
    {
        public static ImageSettings Settings
            => new("sat24microphysics",
                   "Sat24 Nightmicrophysics",
                   new Uri("https://www.sat24.com/en-gb/country/se/hd#selectedLayer=euMicro"),
                   Constants.NotAvailableImageUri);

        public async Task ExecuteAsync()
        {
            var satelliteUrl = await GetFullImageUrlAsync();
            using var satelliteImage = await imageDownloader.DownloadImageAsync(satelliteUrl);

            const string countryBordersUrl = "https://maptiler.infoplaza.io/api/maps/Border/static/11.18,61.64,3.5/1176x882.png?attribution=false";
            using var countryBordersImage = await imageDownloader.DownloadImageAsync(new Uri(countryBordersUrl, UriKind.Absolute));

            // Align border image to satellite image
            countryBordersImage.Mutate(ctx =>
            {
                ctx.Resize(new ResizeOptions
                {
                    Size = satelliteImage.Size,
                    Mode = ResizeMode.BoxPad,
                    Position = AnchorPositionMode.Center,
                });

                ctx.Resize(new ResizeOptions
                {
                    Size = (Size)(1.4f * ctx.GetCurrentSize()),
                    Mode = ResizeMode.Stretch,
                    Position = AnchorPositionMode.Center,
                });

                var cropCorner = new Point((ctx.GetCurrentSize() - satelliteImage.Size) / 2);
                ctx.Crop(new Rectangle(cropCorner, satelliteImage.Size));
            });

            // Add country borders to satellite image and crop excess
            using var outputImage = satelliteImage.Clone(ctx =>
            {
                ctx.DrawImage(countryBordersImage, PixelColorBlendingMode.Add, 1.0f);
                ctx.Crop(new Rectangle(700, 200, 1000, 900));
            });

            await imageUpdater.UpdateImageAsync(Settings.Id, outputImage);
        }

        private async Task<Uri> GetFullImageUrlAsync()
        {
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var address = "https://www.sat24.com/en-gb/country/se/hd";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);

            var scriptTag = document.QuerySelector("""div[data-component="SnippetSatellite"] > div.js-static-satellite > script:nth-child(1)""");
            var scriptContent = scriptTag?.TextContent ?? throw new InvalidOperationException("Could not find script tag in Sat24 page");

            // Extract the radarAvaliableLayers object for the euMicro layer
            // The script is all on one line: radarAvaliableLayers[0]['euMicro'] = { ... };radarAvaliableLayers[0]['next'] = ...
            // Find the start of the euMicro assignment
            var euMicroPattern = @"radarAvaliableLayers\[0\]\['euMicro'\]\s*=\s*";
            var match = Regex.Match(scriptContent, euMicroPattern);
            if (!match.Success)
            {
                throw new InvalidOperationException("Could not find radarAvaliableLayers['euMicro'] in script content");
            }

            // Extract the balanced JavaScript object starting from the match position
            var jsObject = ExtractBalancedBraces(scriptContent, match.Index + match.Length);

            // Convert JavaScript object literal to valid JSON
            var jsonString = ConvertJavaScriptObjectToJson(jsObject);

            // Deserialize JSON to strongly-typed object
            var layerData = JsonSerializer.Deserialize<RadarLayerData>(jsonString)
                ?? throw new InvalidOperationException("Failed to deserialize euMicro layer data");

            // Validate that we have the required data
            if (string.IsNullOrEmpty(layerData.BaseImageUrl))
            {
                throw new InvalidOperationException("baseImageUrl is missing from euMicro layer data");
            }

            if (layerData.RadarLayers.Length == 0)
            {
                throw new InvalidOperationException("No radar layers found in euMicro data");
            }

            // Get the last URL (most recent image)
            var latestImagePath = layerData.RadarLayers[^1].Url;

            // Ensure baseImageUrl ends with a slash for proper URI combination
            var baseImageUrl = layerData.BaseImageUrl;
            if (!baseImageUrl.EndsWith('/'))
            {
                baseImageUrl += "/";
            }

            /* If path starts with a slash, cambining it with the baseImageUrl using the
             * Uri constructor erases the path in the baseImageUrl, resulting in the wrong URL */
            latestImagePath = latestImagePath.StartsWith('/') ? latestImagePath[1..] : latestImagePath;

            // Combine base URL and image path
            var fullUrl = new Uri(new Uri(baseImageUrl), latestImagePath);
            return fullUrl;
        }

        /// <summary>
        /// Extracts a JavaScript object by finding balanced braces starting from a position.
        /// </summary>
        private static string ExtractBalancedBraces(string content, int startIndex)
        {
            int braceCount = 0;
            int objectStart = -1;

            for (int i = startIndex; i < content.Length; i++)
            {
                if (content[i] == '{')
                {
                    if (braceCount == 0)
                    {
                        objectStart = i;
                    }
                    braceCount++;
                }
                else if (content[i] == '}')
                {
                    braceCount--;
                    if (braceCount == 0 && objectStart != -1)
                    {
                        // Found matching closing brace
                        return content.Substring(objectStart, i - objectStart + 1);
                    }
                }
            }

            throw new InvalidOperationException("Could not find balanced braces in JavaScript content");
        }

        /// <summary>
        /// Converts a JavaScript object literal to valid JSON string.
        /// Handles unquoted property names and converts single quotes to double quotes.
        /// </summary>
        private static string ConvertJavaScriptObjectToJson(string jsObject)
        {
            // Step 1: Add quotes around unquoted property names
            // Match: word characters followed by colon (but not already quoted)
            // This pattern looks for property names like: propertyName: or property123:
            var quotedProperties = Regex.Replace(
                jsObject,
                @"(?<![""'])(\b[a-zA-Z_$][a-zA-Z0-9_$]*\b)(?=\s*:)",
                "\"$1\""
            );

            // Step 2: Replace single quotes with double quotes for string values
            // This is a simplified approach - it assumes single quotes are used for strings
            // and not in other contexts (which is typically true for the sat24.com data)
            var withDoubleQuotes = quotedProperties.Replace("'", "\"");

            // Step 3: Remove trailing commas before closing braces/brackets
            var withoutTrailingCommas = Regex.Replace(
                withDoubleQuotes,
                @",(\s*[}\]])",
                "$1"
            );

            return withoutTrailingCommas;
        }
    }

    // JSON models for deserializing the radar layer data
    internal class RadarLayerData
    {
        [JsonPropertyName("baseImageUrl")]
        public string BaseImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("radarLayers")]
        public RadarLayer[] RadarLayers { get; set; } = [];
    }

    internal class RadarLayer
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("layername")]
        public string LayerName { get; set; } = string.Empty;

        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; } = string.Empty;

        [JsonPropertyName("tileurl")]
        public string? TileUrl { get; set; }
    }
}
