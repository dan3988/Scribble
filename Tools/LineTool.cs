using SkiaSharp;

namespace Scribble.Tools;

//public sealed class LineToolType : ScribbleToolType
//{
//	public static readonly LineToolType Instance = new();

//	private LineToolType() : base("line", "Line")
//	{
//	}

//	public override LineTool CreateTool()
//		=> new();
//}

public sealed class LineTool : ScribbleTool
{
	public static readonly LineTool Instance = new();

	private LineTool() : base("line", "Line")
	{
	}

	public override IScribbleAction Begin(SKPoint point, SKColor color, int size)
	{
		var paint = CreateStrokePaint(color, size);
		return new Action(paint, point);
	}

	private class Element : IScribbleElement
	{
		internal readonly SKPaint mPaint;
		internal SKPoint mStart, mEnd;

		public Element(SKPaint paint, SKPoint start, SKPoint end)
		{
			mPaint = paint;
			mStart = start;
			mEnd = end;
		}

		void IDisposable.Dispose()
		{
			mPaint.Dispose();
		}

		void IScribbleElement.Draw(SKCanvas canvas, SKImageInfo canvasInfo)
			=> canvas.DrawLine(mStart, mEnd, mPaint);
	}

	private sealed class Action : Element, IScribbleAction
	{
		public Action(SKPaint paint, SKPoint start) : base(paint, start, start)
		{
		}

		public IScribbleElement Complete(SKPoint point)
			=> new Element(mPaint.Clone(), mStart, point);

		public bool OnDrag(SKPoint point)
		{
			if (mEnd == point)
				return false;

			mEnd = point;
			return true;
		}
	}
}
