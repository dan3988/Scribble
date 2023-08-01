using SkiaSharp;

namespace Scribble.Helpers;

public static class SkiaSharpExtensions
{
	public static void Deconstruct(this SKPoint point, out float x, out float y)
		=> (x, y) = (point.X, point.Y);

	public static void Deconstruct(this SKPointI point, out int x, out int y)
		=> (x, y) = (point.X, point.Y);

	public static void Deconstruct(this SKSize point, out float width, out float height)
		=> (width, height) = (point.Width, point.Height);

	public static void Deconstruct(this SKSizeI point, out int width, out int height)
		=> (width, height) = (point.Width, point.Height);

	public static SKPoint Midpoint(this SKPoint a, SKPoint b)
	{
		var (x1, y1) = a;
		var (x2, y2) = b;
		var dx = x2 - x1;
		var dy = y2 - y1;
		return new(x1 + (dx / 2), y1 + (dy / 2));
	}

	public static SKPointI Midpoint(this SKPointI a, SKPointI b)
	{
		var (x1, y1) = a;
		var (x2, y2) = b;
		var dx = x2 - x1;
		var dy = y2 - y1;
		return new(x1 + (dx / 2), y1 + (dy / 2));
	}

	public static SKPoint Midpoint(this SKPoint a, SKPoint b, out float distance)
	{
		var (x1, y1) = a;
		var (x2, y2) = b;
		var dx = x2 - x1;
		var dy = y2 - y1;
		distance = MathF.Sqrt((dx * dx) + (dy * dy));
		return new(x1 + (dx / 2), y1 + (dy / 2));
	}
}
