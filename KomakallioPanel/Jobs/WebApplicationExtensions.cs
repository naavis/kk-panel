using Hangfire;

namespace KomakallioPanel.Jobs
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureJobs(this WebApplication app)
        {
            var imageManager = app.Services.GetRequiredService<IImageManager>();
            imageManager.Add(new ImageSettings("metsahovi",
                                               "Metsähovi",
                                               new Uri("https://www.metsahovi.fi/allsky-gallery"),
                                               new Uri("https://data.metsahovi.fi/allsky/latest_hf")),
                             "30 */3 * * * *");
            imageManager.Add(new ImageSettings("hankasalmi",
                                               "Hankasalmi Allsky",
                                               new Uri("http://www.ursa.fi/yhd/sirius/sivut/"),
                                               new Uri("http://murtoinen.jklsirius.fi/ccd/skywatch/ImageLastFTP_AllSKY.jpg")),
                             "30 * * * * *");
        }
    }
}
