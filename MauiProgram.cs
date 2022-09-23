using Scribble.Media;

using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Scribble;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
			.ConfigureImageSources(v =>
			{
				v.AddService(svc => new FilteredImageSourceService(svc.GetRequiredService<IImageSourceServiceProvider>()));
			})
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}
