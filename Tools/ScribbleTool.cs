using SkiaSharp;

namespace Scribble.Tools;

public abstract class ScribbleTool
{
	public static readonly ScribbleTool Line = LineTool.Instance;
	public static readonly ScribbleTool Pen = PenTool.Instance;

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
}
