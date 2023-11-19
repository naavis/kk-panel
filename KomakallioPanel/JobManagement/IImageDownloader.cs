namespace KomakallioPanel.JobManagement
{
    public interface IImageDownloader
    {
        Task DownloadImageAsync(string jobId, Uri imageSource, bool onlyLatest = true);
    }
}
