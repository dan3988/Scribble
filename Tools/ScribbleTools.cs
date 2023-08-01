﻿namespace Scribble.Tools;

public static class ScribbleTools
{
	public static readonly ScribbleTool Pen = PenTool.Instance;
	public static readonly ScribbleTool Line = LineTool.Instance;

	public static readonly IReadOnlyList<ScribbleTool> All = new ScribbleTool[]
	{
		Pen, Line
	};
}
