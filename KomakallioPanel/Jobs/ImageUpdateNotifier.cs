
namespace KomakallioPanel.Jobs
{
	public class ImageUpdateNotifier : IImageUpdateNotifier
	{
		private readonly IDictionary<string, List<Action>> callbacks = new Dictionary<string, List<Action>>();
		private readonly ILogger<ImageUpdateNotifier> logger;

		public ImageUpdateNotifier(ILogger<ImageUpdateNotifier> logger)
		{
			this.logger = logger;
		}

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
				catch (Exception ex)
				{
					logger.LogError("Callback for key '{key}' threw an exception: '{exception}'", key, ex.Message);
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
