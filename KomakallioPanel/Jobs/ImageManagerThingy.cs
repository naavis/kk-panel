using Hangfire;

namespace KomakallioPanel.Jobs
{
	public record ImageSettings(string Id, string DisplayName, Uri SourcePage, Uri ImageSource);

	public interface IImageManagerThingy
	{
		void Add(ImageSettings image, string cronSchedule);
		List<ImageSettings> GetImages();
	}

	public class ImageManagerThingy : IImageManagerThingy
	{
		private readonly IRecurringJobManager backgroundJobs;
		private readonly List<ImageSettings> images = new();

		public ImageManagerThingy(IRecurringJobManager backgroundJobs)
		{
			this.backgroundJobs = backgroundJobs;
		}

		public void Add(ImageSettings image, string cronSchedule)
		{
			images.Add(image);
			backgroundJobs.AddOrUpdate<ImageDownloadJob>(
				image.Id,
				job => job.ExecuteAsync(image.Id, image.ImageSource, true),
				cronSchedule);
			backgroundJobs.Trigger(image.Id);
		}

		public List<ImageSettings> GetImages() => images;
	}
}
