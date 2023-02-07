namespace Scribble.Media;

public interface IWrapperImageSource
{
	ImageSource Source { get; }
}

public abstract class WrapperImageSource : ImageSource, IWrapperImageSource
{
	protected static void OnSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		=> ((WrapperImageSource)bindable).OnSourceChanged();

	public static readonly BindableProperty SourceProperty = BindableProperty.Create(
		nameof(Source),
		typeof(ImageSource),
		typeof(FilteredImageSource),
		null,
		propertyChanged: OnSourcePropertyChanged);

	public ImageSource Source
	{
		get => (ImageSource)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	public override bool IsEmpty => GetValue(SourceProperty) == null;
}
