namespace Scribble.Media;

public partial class FilteredImageSourceService : WrapperImageSourceService<FilteredImageSource>, IImageSourceService<IFilteredImageSource>
{
	public FilteredImageSourceService(IImageSourceServiceProvider provider) : base(provider)
	{
	}
}