using UIKit;

namespace Scribble.Media;

partial class WrapperImageSourceService<T>
{
	protected abstract bool WillModifyImage(T source);

	protected abstract ImageSourceServiceResult ModifyImage(T source, float scale, UIImage image);

	public override Task<IImageSourceServiceResult<UIImage>> GetImageAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
	{
		var src = (T)imageSource;
		var wrapped = src.Source;
		if (wrapped == null)
		{
			return Task.FromResult<IImageSourceServiceResult<UIImage>>(null);
		}
		else
		{
			var handler = mProvider.GetRequiredImageSourceService(wrapped);
			var task = handler.GetImageAsync(wrapped, scale, cancellationToken);

			if (WillModifyImage(src))
			{
				task = task.ContinueWith<IImageSourceServiceResult<UIImage>>(t =>
				{
					using var result = t.GetAwaiter().GetResult();
					return ModifyImage(src, scale, result.Value);
				});
			}

			return task;
		}
	}
}
