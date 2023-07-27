using System.Windows.Input;

using Scribble.Helpers;

using SkiaSharp;

namespace Scribble.ViewModels;

public sealed class MainViewModel : AbstractViewModel
{
	private static SKBitmap EmptyBitmap(SKColor color, SKSizeI size)
	{
		var bitmap = new SKBitmap(size.Width, size.Height);

		using var canvas = new SKCanvas(bitmap);
		using var paint = new SKPaint()
		{
			Color = color
		};

		canvas.DrawRect(0, 0, size.Width, size.Height, paint);

		return bitmap;
	}

	private static readonly SKSizeI DefaultSize = new SKSizeI(900, 900);

	public ICommand OpenCommand { get; }

	private ScribbleModel _currentEdit;
	public ScribbleModel CurrentEdit
	{
		get => _currentEdit;
		private set => this.SetPropertyValueRef(ref _currentEdit, value);
	}

	public MainViewModel()
	{
		var image = EmptyBitmap(SKColors.White, DefaultSize);
		_currentEdit = new(this, image);
		OpenCommand = new Command(OpenFile);
	}

	private async void OpenFile()
	{
		var file = await FilePicker.PickAsync(new PickOptions
		{
			FileTypes = FilePickerFileType.Images
		});

		if (file != null)
		{
			var bitmap = SKBitmap.Decode(file.FullPath);
			CurrentEdit = new(this, bitmap);
		}
	}
}
