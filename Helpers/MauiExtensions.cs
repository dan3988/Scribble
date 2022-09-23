namespace Scribble.Helpers;

public static class MauiExtensions
{
	public static Rect Inflate(this in Rect rect, double left, double top, double right, double bottom)
		=> new(rect.X + left, rect.Y + top, rect.Width - left - right, rect.Height - top - bottom);

	public static Rect Deflate(this in Rect rect, double left, double top, double right, double bottom)
		=> new(rect.X - left, rect.Y - top, rect.Width + left + right, rect.Height + top + bottom);

	public static Rect Inflate(this in Rect rect, in Thickness thickness)
	{
		var (l, t, r, b) = thickness;
		return Inflate(in rect, l, t, r, b);
	}

	public static Rect Deflate(this in Rect rect, in Thickness thickness)
	{
		var (l, t, r, b) = thickness;
		return Deflate(in rect, l, t, r, b);
	}
}
