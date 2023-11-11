namespace KomakallioPanel.JobManagement
{
    public interface IImageManager
    {
        void Add(ImageSettings image, string cronSchedule);
        List<ImageSettings> GetImages();

        public event Action<string>? ListChanged;

        public void NotifyListChanged(string key);
    }
}
