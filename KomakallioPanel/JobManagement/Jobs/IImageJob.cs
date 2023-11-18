namespace KomakallioPanel.JobManagement.Jobs
{
    public interface IImageJob
    {
        static abstract ImageSettings Settings { get; }

        Task ExecuteAsync();
    }
}
