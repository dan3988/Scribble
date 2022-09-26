using System.Security.Cryptography;

using Scribble.Helpers;
using Scribble.Tools;

using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Scribble;

public partial class ScribbleView
{
	private static SKBitmap EmptyBitmap(SKColor color, SKSizeI size)
	{
		var bitmap = new SKBitmap(size.Width, size.Height);

		using var canvas = new SKCanvas(bitmap);
		using var paint = new SKPaint()
		{
			Color = color
		};

		canvas.DrawRect(0, 0, size.Width, size.Height, paint);

		return bitmap;
	}

	private static readonly SKSizeI DefaultSize = new SKSizeI(900, 900);

	private readonly Dictionary<long, IScribbleAction> mActions;
	private readonly SKBitmap mBackground;
	private ScribbleModel mModel;
	private float mCanvasScale;
	private float mDpi;

	public ScribbleView(SKBitmap bitmap)
	{
		mActions = new();
		mBackground = bitmap;
		InitializeComponent();
	}

	public ScribbleView(SKColor color, SKSizeI size) : this(EmptyBitmap(color, size))
	{
	}

	public ScribbleView() : this(SKColors.White, DefaultSize)
	{
	}

	protected override Size ArrangeOverride(Rect bounds)
	{
		base.ArrangeOverride(bounds);

		var padding = Padding;
		var area = bounds.Inflate(padding);
		var info = mBackground.Info;
		var scaleX = area.Width / info.Width;
		var scaleY = area.Height / info.Height;
		var scale = Math.Min(scaleX, scaleY);

		var w = info.Width * scale;
		var h = info.Height * scale;

		area = new(area.X + ((area.Width - w) / 2), area.Y + ((area.Height - h) / 2), w, h);

		mCanvas.Arrange(area);
		mDpi = (float)DeviceDisplay.MainDisplayInfo.Density;
		mCanvasScale = (float)scale;

		return bounds.Size;
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();

		if (mModel != null)
			mModel.Invalidated -= OnModelInvalidated;

		mModel = BindingContext as ScribbleModel;
		
		if (mModel != null)
			mModel.Invalidated += OnModelInvalidated;

	}

	private void OnModelInvalidated(object sender, EventArgs e)
	{
		mCanvas.InvalidateSurface();
	}

	protected override SimpleLayoutManager CreateLayoutManager()
		=> new(this);

	private void OnPaint(object sender, SKPaintSurfaceEventArgs e)
	{
		if (mModel == null)
			return;

		var canvas = e.Surface.Canvas;
		var info = e.Info;
		var (cx, cy, cw, ch) = mCanvas.Bounds;
		canvas.Translate((float)(cx * mDpi), (float)(cy * mDpi));
		canvas.Scale(mCanvasScale * mDpi);
		canvas.Clear(SKColors.Transparent);
		canvas.DrawBitmap(mBackground, SKPoint.Empty);

		foreach (var action in mActions.Values)
			action.Draw(canvas, info);

		canvas.ClipRect(new SKRect(0, 0, (float)(cw / mCanvasScale), (float)(ch / mCanvasScale)));

		foreach (var element in mModel.Elements)
			element.Draw(canvas, info);
	}

	private void OnTouch(object sender, SKTouchEventArgs e)
	{
		if (mModel == null)
			return;

		var point = e.Location;
		var scale = mCanvasScale * mDpi;
		point.X = (float)(point.X - (mCanvas.X * mDpi)) / scale;
		point.Y = (float)(point.Y - (mCanvas.Y * mDpi)) / scale;

		switch (e.ActionType)
		{
			case SKTouchAction.Pressed:
			{
				var action = mModel.BeginAction(point);
				if (action == null)
					return;

				if (mActions.TryAdd(e.Id, action))
					mCanvas.InvalidateSurface();

				break;
			}
			case SKTouchAction.Moved:
			{
				if (mActions.TryGetValue(e.Id, out var action) && action.OnDrag(point))
					mCanvas.InvalidateSurface();

				break;
			}
			case SKTouchAction.Released:
			{
				if (mActions.Remove(e.Id, out var action))
				{
					var result = action.Complete(point);
					mModel.AddElement(result);
					mCanvas.InvalidateSurface();
				}

				break;
			}
			case SKTouchAction.Cancelled:
			{
				if (mActions.Remove(e.Id, out var action))
				{
					action.Dispose();
					mCanvas.InvalidateSurface();
				}

				break;
			}
		}

		e.Handled = true;
	}
}