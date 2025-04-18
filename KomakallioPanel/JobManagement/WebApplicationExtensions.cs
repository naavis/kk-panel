﻿using KomakallioPanel.JobManagement.Jobs;

namespace KomakallioPanel.JobManagement
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureJobs(this WebApplication app)
        {
            var imageManager = app.Services.GetRequiredService<IImageManager>();
            imageManager.Add<KomakallioJob>("*/15 * * * * *");
            imageManager.Add<FmiTestbedJob>("0 */5 * * * *");
            imageManager.Add<Sat24MicrophysicsJob>("30 */5 * * * *");
            imageManager.Add<MetsahoviJob>("30 */3 * * * *");
            imageManager.Add<BromarvJob>("30 * * * * *");
            imageManager.Add<TahtikallioJob>("30 */5 * * * *");
            imageManager.Add<KevolaJob>("0,30 * * * * *");
            imageManager.Add<TampereNorthJob>("30 * * * * *");
            imageManager.Add<TampereSouthJob>("30 * * * * *");
            imageManager.Add<HankasalmiJob>("30 * * * * *");
        }
    }
}
