namespace KomakallioPanel.Jobs
{
    public interface IImageManager
    {
        void Add(ImageSettings image, string cronSchedule);
        List<ImageSettings> GetImages();

        public event Action? ListChanged;

        public void NotifyListChanged();
    }
}
