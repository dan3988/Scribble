using System.Windows.Input;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;

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

	private static readonly SKSizeI DefaultSize = new(900, 900);

	public ICommand OpenCommand { get; }

	public ICommand SaveCommand { get; }

	private ScribbleModel mCurrentEdit;
	public ScribbleModel CurrentEdit
	{
		get => mCurrentEdit;
		private set => this.SetPropertyValueRef(ref mCurrentEdit, value);
	}

	public MainViewModel()
	{
		var image = EmptyBitmap(SKColors.White, DefaultSize);
		mCurrentEdit = new(this, image);
		OpenCommand = new Command(OpenFile);
		SaveCommand = new Command(Save);
	}

	private async void Save()
	{
		var temp = Path.GetTempFileName();
		var path = default(string);

		try
		{
			using var stream = File.Open(temp, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

			mCurrentEdit.Save(stream);

			stream.Position = 0;

			var result = await FileSaver.Default.SaveAsync("image.png", stream, CancellationToken.None);
			path = result.FilePath;
		}
		finally
		{
			File.Delete(temp);
		}

		if (path != null)
		{
			var file = new ReadOnlyFile(path, "image/png");

			async void Show()
			{
				if (!await Launcher.OpenAsync(new OpenFileRequest("Image", file)))
				{
					using var toast = Toast.Make("Failed to open " + path);
					await toast.Show();
				}
			}

			using var snack = Snackbar.Make("Image saved successfully", Show, "Open");
			await snack.Show();
		}
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
