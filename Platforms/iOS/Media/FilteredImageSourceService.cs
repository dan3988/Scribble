using UIKit;

namespace Scribble.Media;

partial class FilteredImageSourceService
{
	protected override bool WillModifyImage(FilteredImageSource source)
		=> source.Color != null;

	protected override ImageSourceServiceResult ModifyImage(FilteredImageSource source, float scale, UIImage image)
	{
		return new(image);
	}
}
