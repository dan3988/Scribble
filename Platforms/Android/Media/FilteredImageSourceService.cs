using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;

using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace Scribble.Media;

partial class FilteredImageSourceService
{
	protected override bool WillModifyImage(FilteredImageSource source)
		=> source.Color != null;

	protected override ImageSourceServiceResult ModifyImage(FilteredImageSource source, Context context, Drawable drawable)
	{
		var color = source.Color;
		var filter = new PorterDuffColorFilter(color.ToAndroid(), PorterDuff.Mode.SrcIn);

		drawable.SetColorFilter(filter);

		return new ImageSourceServiceResult(drawable, () =>
		{
			drawable.Dispose();
			filter.Dispose();
		});
	}
}
