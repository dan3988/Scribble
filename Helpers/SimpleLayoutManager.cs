using Microsoft.Maui.Layouts;

namespace Scribble.Helpers;

/// <summary>
/// Uses the entire bounds of the parent layout for each child view.
/// </summary>
public sealed class SimpleLayoutManager : ILayoutManager
{
	private readonly Layout mLayout;

	public SimpleLayoutManager(Layout mLayout)
	{
		this.mLayout = mLayout;
	}

	public Size ArrangeChildren(Rect bounds)
	{
		var padding = mLayout.Padding;
		var available = bounds.Inflate(padding);

		var maxW = 0D;
		var maxH = 0D;

		foreach (var child in mLayout.Children)
		{
			var (w, h) = child.Arrange(available);

			if (maxW < w)
				maxW = w;

			if (maxH < h)
				maxH = h;
		}

		return new Size(maxW + padding.HorizontalThickness, maxH + padding.VerticalThickness);
	}

	public Size Measure(double widthConstraint, double heightConstraint)
	{
		var maxW = 0D;
		var maxH = 0D;
		var padding = mLayout.Padding;

		widthConstraint -= padding.HorizontalThickness;
		heightConstraint -= padding.VerticalThickness;

		foreach (var child in mLayout.Children)
		{
			var (w, h) = child.Measure(widthConstraint, heightConstraint);

			if (maxW < w)
				maxW = w;

			if (maxH < h)
				maxH = h;
		}

		return new(maxW + padding.HorizontalThickness, maxH + padding.VerticalThickness);
	}
}
