using SkiaSharp;

namespace Scribble;

public partial class ScribblePage : ContentPage
{
	public ScribblePage()
	{
		InitializeComponent();
	}

	private async void OpenFile(object sender, EventArgs e)
	{
		var file = await FilePicker.PickAsync(new PickOptions
		{
			FileTypes = FilePickerFileType.Images
		});

		if (file != null)
		{
			var bitmap = SKBitmap.Decode(file.FullPath);
			Content = new ScribbleView(bitmap);
		}
	}
}