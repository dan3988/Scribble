using Android.Content;
using Android.Graphics.Drawables;

namespace Scribble.Media;

partial class WrapperImageSourceService<T>
{
	protected abstract bool WillModifyImage(T source);

	protected abstract ImageSourceServiceResult ModifyImage(T source, Context context, Drawable mutableDrawable);

	public override Task<IImageSourceServiceResult<Drawable>> GetDrawableAsync(IImageSource imageSource, Context context, CancellationToken cancellationToken = default)
	{
		var src = (T)imageSource;
		var wrapped = src.Source;
		if (wrapped == null)
		{
			return Task.FromResult<IImageSourceServiceResult<Drawable>>(null);
		}
		else
		{
			var handler = mProvider.GetRequiredImageSourceService(wrapped);
			var task = handler.GetDrawableAsync(wrapped, context, cancellationToken);

			if (WillModifyImage(src))
			{
				task = task.ContinueWith<IImageSourceServiceResult<Drawable>>(t =>
				{
					using var result = t.GetAwaiter().GetResult();
					using var state = result.Value.GetConstantState();

					var drawable = state.NewDrawable().Mutate();
					return ModifyImage(src, context, drawable);
				});
			}

			return task;
		}
	}
}
