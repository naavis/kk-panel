using KomakallioPanel.JobManagement.Jobs;

namespace KomakallioPanel.JobManagement
{
    public interface IImageManager
    {
        void Add<T>(string cronSchedule) where T : IImageJob;
        List<ImageSettings> GetImages();

        void NotifyListChanged(string key);
        event Action<string>? ListChanged;
    }
}
