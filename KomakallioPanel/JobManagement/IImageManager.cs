using KomakallioPanel.JobManagement.Jobs;

namespace KomakallioPanel.JobManagement
{
    public interface IImageManager
    {
        void Add<T>(string cronSchedule) where T : IImageJob;
        List<ImageSettings> GetImages();

        public event Action<string>? ListChanged;

        public void NotifyListChanged(string key);
    }
}
