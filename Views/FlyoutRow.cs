using System.Windows.Input;

namespace Scribble.Views;

public class FlyoutRow : ContentView
{
	public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(FlyoutItem));

	public ImageSource Icon
	{
		get => (ImageSource)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FlyoutItem));

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FlyoutItem));

	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public FlyoutRow()
	{
	}
}