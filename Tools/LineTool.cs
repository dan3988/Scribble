using SkiaSharp;

namespace Scribble.Tools;

public sealed class LineTool : ScribbleTool
{
	public static readonly LineTool Instance = new();

	private static readonly SimpleDrawHandler mDrawHandler = (canvas, paint, start, end) => canvas.DrawLine(start, end, paint);

	private LineTool() : base("line", "Line")
	{
	}

	public override IScribbleAction Begin(SKPoint point, SKColor color, int size)
	{
		var paint = CreateStrokePaint(color, size);
		return CreateSimpleAction(mDrawHandler, paint, point);
	}
}
