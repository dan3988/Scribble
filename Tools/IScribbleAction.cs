using SkiaSharp;

namespace Scribble.Tools;

public interface IScribbleAction : IScribbleElement
{
	bool OnDrag(SKPoint point);

	IScribbleElement Complete(SKPoint point);
}
