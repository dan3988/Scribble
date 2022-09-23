using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;

using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace Scribble.Media;

partial class FilteredImageSourceService
{
	public override Task<IImageSourceServiceResult<Drawable>> GetDrawableAsync(IImageSource imageSource, Context context, CancellationToken cancellationToken = default)
	{
		var src = (IFilteredImageSource)imageSource;
		var wrapped = src.Source;
		if (wrapped == null)
		{
			return Task.FromResult<IImageSourceServiceResult<Drawable>>(null);
		}
		else
		{
			var handler = mProvider.GetRequiredImageSourceService(wrapped);
			var task = handler.GetDrawableAsync(wrapped, context, cancellationToken);
			var color = src.Color;
			if (color != null)
			{
				task = task.ContinueWith<IImageSourceServiceResult<Drawable>>(t =>
				{
					using var result = t.GetAwaiter().GetResult();
					using var state = result.Value.GetConstantState();

					var drawable = state.NewDrawable().Mutate();
					var filter = new PorterDuffColorFilter(color.ToAndroid(), PorterDuff.Mode.SrcIn);

					drawable.SetColorFilter(filter);

					return new ImageSourceServiceResult(drawable, () =>
					{
						drawable.Dispose();
						filter.Dispose();
					});
				});
			}

			return task;
		}
	}
}
