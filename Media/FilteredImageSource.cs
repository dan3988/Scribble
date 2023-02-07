namespace Scribble.Media;

public interface IFilteredImageSource : IWrapperImageSource, IImageSource
{
	Color Color { get; }
}

public class FilteredImageSource : WrapperImageSource, IFilteredImageSource
{
	public static readonly BindableProperty ColorProperty = BindableProperty.Create(
		nameof(Color),
		typeof(Color),
		typeof(FilteredImageSource),
		null,
		propertyChanged: OnSourcePropertyChanged);

	public Color Color
	{
		get => (Color)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}
}
