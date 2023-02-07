using SkiaSharp;

namespace Scribble.Tools;

public sealed class PenTool : ScribbleTool
{
	public static readonly PenTool Instance = new();

	private PenTool() : base("pen", "Pen")
	{
	}

	public override IScribbleAction Begin(SKPoint point, SKColor color, int size)
	{
		var paint = CreateStrokePaint(color, size);
		var path = new SKPath();
		path.MoveTo(point);
		return new Action(paint, path);
	}

	private class Element : IScribbleElement
	{
		internal readonly SKPaint mPaint;
		internal readonly SKPath mPath;

		public Element(SKPaint paint, SKPath path)
		{
			mPaint = paint;
			mPath = path;
		}

		void IDisposable.Dispose()
		{
			mPaint.Dispose();
			mPath.Dispose();
		}

		void IScribbleElement.Draw(SKCanvas canvas, SKImageInfo canvasInfo)
			=> canvas.DrawPath(mPath, mPaint);
	}

	private sealed class Action : Element, IScribbleAction
	{
		public Action(SKPaint paint, SKPath path) : base(paint, path)
		{
		}

		public IScribbleElement Complete(SKPoint point)
		{
			var clone = new SKPath(mPath);
			clone.MoveTo(point);
			return new Element(mPaint.Clone(), clone);
		}

		public bool OnDrag(SKPoint point)
		{
			mPath.LineTo(point);
			return true;
		}
	}
}
