using SkiaSharp;

namespace Scribble.Tools;

public abstract class ScribbleTool
{
	protected delegate void SimpleDrawHandler(SKCanvas canvas, SKPaint paint, SKPoint start, SKPoint end);

	protected static IScribbleAction CreateSimpleAction(SimpleDrawHandler handler, SKPaint paint, SKPoint start)
		=> new SimpleAction(handler, paint, start);

	protected static SKPaint CreateStrokePaint(SKColor color, int size) => new()
	{
		Color = color,
		StrokeWidth = size,
		IsAntialias = true,
		StrokeCap = SKStrokeCap.Round,
		StrokeJoin = SKStrokeJoin.Round,
		Style = SKPaintStyle.Stroke
	};

	public string Id { get; }

	public string Name { get; }

	public ImageSource Icon { get; }

	protected ScribbleTool(string id, string name) : this(id, name, $"tool_{id}.svg")
	{
	}

	protected ScribbleTool(string id, string name, ImageSource icon)
	{
		Id = id;
		Name = name;
		Icon = icon;
	}

	public abstract IScribbleAction Begin(SKPoint point, SKColor color, int size);


	private class SimpleElement : IScribbleElement
	{
		internal readonly SimpleDrawHandler mHandler;
		internal readonly SKPaint mPaint;
		internal SKPoint mStart, mEnd;

		public SimpleElement(SimpleDrawHandler handler, SKPaint paint, SKPoint start, SKPoint end)
		{
			mHandler = handler;
			mPaint = paint;
			mStart = start;
			mEnd = end;
		}

		void IDisposable.Dispose()
		{
			mPaint.Dispose();
		}

		void IScribbleElement.Draw(SKCanvas canvas, SKImageInfo canvasInfo)
			=> mHandler.Invoke(canvas, mPaint, mStart, mEnd);
	}

	private sealed class SimpleAction : SimpleElement, IScribbleAction
	{
		public SimpleAction(SimpleDrawHandler handler, SKPaint paint, SKPoint start) : base(handler, paint, start, start)
		{
		}

		public IScribbleElement Complete(SKPoint point)
			=> new SimpleElement(mHandler, mPaint.Clone(), mStart, point);

		public bool OnDrag(SKPoint point)
		{
			if (mEnd == point)
				return false;

			mEnd = point;
			return true;
		}
	}
}
