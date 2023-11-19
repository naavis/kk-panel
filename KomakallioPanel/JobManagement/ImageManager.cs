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

        public void Update(string id, Uri newImageRelativeUri)
        {
            var imageIndex = images.FindIndex(x => x.Id == id);
            images[imageIndex] = images[imageIndex] with { ImagePath = newImageRelativeUri };
            NotifyListChanged();
        }

        public List<ImageSettings> GetImages() => images;

        public event Action? ListChanged;

        private void NotifyListChanged()
        {
            ListChanged?.Invoke();
        }
    }
}
