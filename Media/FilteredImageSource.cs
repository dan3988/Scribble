using Microsoft.Maui.Controls.Compatibility;

using Scribble.Media;

namespace Scribble.Media;

public interface IFilteredImageSource : IImageSource
{
	ImageSource Source { get; }

	Color Color { get; }
}

public class FilteredImageSource : ImageSource, IFilteredImageSource
{
	private static void Reset(BindableObject bindable, object oldValue, object newValue)
		=> ((FilteredImageSource)bindable).OnSourceChanged();

	public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(FilteredImageSource), null, propertyChanged: Reset);

	public ImageSource Source
	{
		get => (ImageSource)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(FilteredImageSource), null, propertyChanged: Reset);

	public Color Color
	{
		get => (Color)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	public override bool IsEmpty => GetValue(SourceProperty) == null;
}

public partial class FilteredImageSourceService : ImageSourceService, IImageSourceService<IFilteredImageSource>
{
	private readonly IImageSourceServiceProvider mProvider;

	public FilteredImageSourceService(IImageSourceServiceProvider provider)
	{
		mProvider = provider;
	}
}