namespace KomakallioPanel.JobManagement
{
    public interface IImageUpdater
    {
        Task UpdateImageAsync(string jobId, Uri imageSource);
        Task UpdateImageAsync(string jobId, Image image);
    }
}
