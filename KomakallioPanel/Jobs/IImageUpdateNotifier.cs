namespace KomakallioPanel.Jobs
{
	public interface IImageUpdateNotifier
	{
		void Changed(string key, string outputFilename);
		void RegisterCallback(string key, Action callback);
	}
}