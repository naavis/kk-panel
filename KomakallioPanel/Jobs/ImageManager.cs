using Hangfire;

namespace KomakallioPanel.Jobs
{
    public class ImageManager : IImageManager
    {
        private readonly IRecurringJobManager backgroundJobs;
        private readonly List<ImageSettings> images = new();

        public ImageManager(IRecurringJobManager backgroundJobs)
        {
            this.backgroundJobs = backgroundJobs;
        }

        public void Add(ImageSettings image, string cronSchedule)
        {
            images.Add(image);
            backgroundJobs.AddOrUpdate<ImageDownloadJob>(
                image.Id,
                job => job.ExecuteAsync(image.Id, image.ImageSource, true),
                cronSchedule);
            backgroundJobs.Trigger(image.Id);
        }

        public List<ImageSettings> GetImages() => images;

        public event Action? ListChanged;

        public void NotifyListChanged()
        {
            ListChanged?.Invoke();
        }
    }
}
