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
		return new LineAction(paint, point);
	}

	private class LineElement : IScribbleElement
	{
		internal readonly SKPaint paint;
		internal SKPoint start, end;

		public LineElement(SKPaint paint, SKPoint start, SKPoint end)
		{
			this.paint = paint;
			this.start = start;
			this.end = end;
		}

		void IDisposable.Dispose()
		{
			paint.Dispose();
		}

		void IScribbleElement.Draw(SKCanvas canvas, SKImageInfo canvasInfo)
			=> canvas.DrawLine(start, end, paint);
	}

	private sealed class LineAction : LineElement, IScribbleAction
	{
		public LineAction(SKPaint paint, SKPoint start) : base(paint, start, start)
		{
		}

		public IScribbleElement Complete(SKPoint point)
			=> new LineElement(paint, start, point);

		public bool OnDrag(SKPoint point)
		{
			if (end == point)
				return false;

			end = point;
			return true;
		}
	}
}
