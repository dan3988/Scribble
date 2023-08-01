using Scribble.Helpers;

using SkiaSharp;

namespace Scribble.Tools;

public sealed class RectangleTool : ScribbleTool
{
	public static readonly RectangleTool Instance = new();

	private static readonly SimpleDrawHandler mDrawHandler = (canvas, paint, start, end) =>
	{
		var (x1, y1) = start;
		var (width, height) = end - start;
		canvas.DrawRect(x1, y1, width, height, paint);
	};

	private RectangleTool() : base("rect", "Rectangle")
	{
	}

	public override IScribbleAction Begin(SKPoint point, SKColor color, int size)
	{
		var paint = CreateStrokePaint(color, size);
		return CreateSimpleAction(mDrawHandler, paint, point);
	}
}
