namespace Scribble.Media;

public abstract partial class WrapperImageSourceService<T> : ImageSourceService
	where T : IWrapperImageSource, IImageSource
{
	protected readonly IImageSourceServiceProvider mProvider;

	public WrapperImageSourceService(IImageSourceServiceProvider provider)
	{
		mProvider = provider;
	}
}