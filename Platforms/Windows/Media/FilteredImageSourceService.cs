using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Scribble.Media;

using ImageSource = Microsoft.UI.Xaml.Media.ImageSource;

partial class FilteredImageSourceService
{
	public override Task<IImageSourceServiceResult<ImageSource>> GetImageSourceAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
	{
		var src = (IFilteredImageSource)imageSource;
		var wrapped = src.Source;
		if (wrapped == null)
		{
			return Task.FromResult<IImageSourceServiceResult<ImageSource>>(null);
		}
		else
		{
			var handler = mProvider.GetRequiredImageSourceService(wrapped);
			var task = handler.GetImageSourceAsync(wrapped, scale, cancellationToken);
			var color = src.Color;
			if (color != null)
			{
				//task = task.ContinueWith<IImageSourceServiceResult<ImageSource>>(t =>
				//{
				//	using var result = t.GetAwaiter().GetResult();

				//	var bitmap = new Lumia()

				//	return new ImageSourceServiceResult(drawable, () =>
				//	{
				//		drawable.Dispose();
				//		filter.Dispose();
				//	});
				//});
			}

			return task;
		}
	}
}
