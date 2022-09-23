using SkiaSharp;

namespace Scribble.Tools;

public interface IScribbleElement : IDisposable
{
	void Draw(SKCanvas canvas, SKImageInfo canvasInfo);
}
