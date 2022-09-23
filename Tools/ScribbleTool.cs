using SkiaSharp;

namespace Scribble.Tools;

public abstract class ScribbleTool
{
	public static readonly ScribbleTool Line = LineTool.Instance;

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

	protected ScribbleTool(string id, string name)
	{
		Id = id;
		Name = name;
	}

	public abstract IScribbleAction Begin(SKPoint point, SKColor color, int size);

	protected abstract class AbstractAction : IScribbleAction
	{
		private readonly SKPaint paint;
		private bool closed;

		protected AbstractAction(SKPaint paint)
		{
			this.paint = paint;
		}

		~AbstractAction()
			=> Dispose(false);

		protected abstract void Dispose(bool disposing);

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected abstract void Draw(SKCanvas canvas, SKImageInfo canvasInfo, SKPaint paint);

		protected abstract bool Move(SKPoint point, bool completing);

		IScribbleElement IScribbleAction.Complete(SKPoint point)
		{
			if (closed)
				throw new InvalidOperationException("Action has been completed.");

			Move(point, true);
			closed = true;
			return this;
		}

		void IScribbleElement.Draw(SKCanvas canvas, SKImageInfo canvasInfo)
			=> Draw(canvas, canvasInfo, paint);

		bool IScribbleAction.OnDrag(SKPoint point)
		{
			if (closed)
				throw new InvalidOperationException("Action has been completed.");

			return Move(point, false);
		}
	}
}
