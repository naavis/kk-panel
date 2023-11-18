using Hangfire;
using KomakallioPanel.JobManagement.Jobs;

namespace KomakallioPanel.JobManagement
{
    public class ImageManager : IImageManager
    {
        private readonly IRecurringJobManager backgroundJobs;
        private readonly List<ImageSettings> images = new();

        public ImageManager(IRecurringJobManager backgroundJobs)
        {
            this.backgroundJobs = backgroundJobs;
        }

        public void Add<T>(string cronSchedule) where T : IImageJob
        {
            var settings = T.Settings;
            images.Add(settings);
            backgroundJobs.AddOrUpdate<T>(settings.Id, job => job.ExecuteAsync(), cronSchedule);
            backgroundJobs.Trigger(settings.Id);
        }

        public List<ImageSettings> GetImages() => images;

        public event Action<string>? ListChanged;

        public void NotifyListChanged(string key)
        {
            ListChanged?.Invoke(key);
        }
    }
}
