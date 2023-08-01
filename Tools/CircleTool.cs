using Scribble.Helpers;

using SkiaSharp;

namespace Scribble.Tools;

public sealed class CircleTool : ScribbleTool
{
	public static readonly CircleTool Instance = new();

	private static readonly SimpleDrawHandler mDrawHandler = (canvas, paint, start, end) =>
	{
		var center = start.Midpoint(end, out var diameter);
		canvas.DrawCircle(center, diameter / 2F, paint);
	};

	private CircleTool() : base("circle", "Circle")
	{
	}

	public override IScribbleAction Begin(SKPoint point, SKColor color, int size)
	{
		var paint = CreateStrokePaint(color, size);
		return CreateSimpleAction(mDrawHandler, paint, point);
	}
}
