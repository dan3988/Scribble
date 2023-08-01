using SkiaSharp;

namespace Scribble.Tools;

public abstract class ScribbleTool
{
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
}
