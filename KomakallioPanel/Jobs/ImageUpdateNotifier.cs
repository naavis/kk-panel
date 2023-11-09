
namespace KomakallioPanel.Jobs
{
	public class ImageUpdateNotifier : IImageUpdateNotifier
	{
		private readonly IDictionary<string, List<Action>> callbacks = new Dictionary<string, List<Action>>();

		public void Changed(string key, string outputFilename)
		{
			if (!callbacks.TryGetValue(key, out List<Action>? callbacksForKey))
			{
				return;
			}

			foreach (var callback in callbacksForKey)
			{
				try
				{
					callback.Invoke();
				}
				catch
				{
					// TODO: Log exceptions from callbacks
				}
			}
		}

		public void RegisterCallback(string key, Action callback)
		{
			if (callbacks.TryGetValue(key, out List<Action>? value))
			{
				value.Add(callback);
			}
			else
			{
				callbacks.Add(key, new List<Action>() { callback });
			}
		}
	}
}
