using KomakallioPanel.JobManagement.Jobs;

namespace KomakallioPanel.JobManagement
{
    public interface IImageManager
    {
        void Add<T>(string cronSchedule) where T : IImageJob;
        void Update(string id, Uri newImageRelativeUri);
        List<ImageSettings> GetImages();

        event Action? ListChanged;
    }
}
