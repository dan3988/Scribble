using System.Windows.Input;
using Scribble.Helpers;
using Scribble.Tools;

using SkiaSharp;

namespace Scribble.ViewModels;

public sealed class ScribbleModel : AbstractViewModel
{
	private sealed class ElementList : AbstractReadOnlyList<IScribbleElement>
	{
		private readonly ScribbleModel mModel;

		public ElementList(ScribbleModel model)
		{
			mModel = model;
		}

		public override IScribbleElement this[int index]
		{
			get
			{
				//throw the default error message
				if (index >= mModel.mElementCount)
					index = mModel.mElements.Count;

				return mModel.mElements[index];
			}
		}

		public override int Count => mModel.mElementCount;

		public override IEnumerator<IScribbleElement> GetEnumerator()
		{
			var ix = 0;
			var count = mModel.mElementCount;
			if (count == 0)
				yield break;

			using var en = mModel.mElements.GetEnumerator();

			while (en.MoveNext())
			{
				yield return en.Current;

				if (++ix == count)
					break;
			}
		}
	}

	public SKSizeI ImageSize { get; }

	private SKColor mColor = SKColors.Black;
	public SKColor Color
	{
		get => mColor;
		set => this.SetPropertyValueEqu(ref mColor, value);
	}

	private int mSize = 12;
	public int Size
	{
		get => mSize;
		set => this.SetPropertyValue(ref mSize, value);
	}

	private ScribbleTool mTool = ScribbleTool.Pen;
	public ScribbleTool Tool
	{
		get => mTool;
		set => this.SetPropertyValueRef(ref mTool, value);
	}

	private bool mCanUndo;
	public bool CanUndo
	{
		get => mCanUndo;
		private set => this.SetPropertyValueBln(ref mCanUndo, value);
	}

	private bool mCanRedo;
	public bool CanRedo
	{
		get => mCanRedo;
		private set => this.SetPropertyValueBln(ref mCanRedo, value);
	}

	private readonly List<IScribbleElement> mElements;
	private int mElementCount;

	public IReadOnlyList<IScribbleElement> Elements { get; }

	public SKBitmap Image { get; }

	public MainViewModel Parent { get; }

	public ICommand UndoCommand { get; }

	public ICommand RedoCommand { get; }

	public event EventHandler Invalidated;

	public ScribbleModel(MainViewModel parent, SKBitmap image)
	{
		mElements = new();
		Parent = parent;
		Image = image;
		ImageSize = new(image.Width, image.Height);
		Elements = new ElementList(this);
		UndoCommand = new Command(() => Undo());
		RedoCommand = new Command(() => Redo());
	}

	public void DrawImage(SKCanvas canvas, SKPoint point)
		=> canvas.DrawBitmap(Image, point);

	public IScribbleAction BeginAction(SKPoint point)
		=> mTool.Begin(point, mColor, mSize);

	public void AddElement(IScribbleElement element)
	{
		var undoCount = mElements.Count - mElementCount;
		if (undoCount > 0)
			mElements.RemoveRange(mElementCount, undoCount);

		mElements.Add(element);
		mElementCount = mElements.Count;
		CanUndo = true;
		CanRedo = false;
		Invalidated?.Invoke(this, EventArgs.Empty);
	}

	public bool Undo()
	{
		if (mElementCount == 0)
			return false;

		CanUndo = --mElementCount > 0;
		CanRedo = true;
		Invalidated?.Invoke(this, EventArgs.Empty);
		return true;
	}

	public bool Redo()
	{
		if (mElementCount == mElements.Count)
			return false;

		CanUndo = true;
		CanRedo = ++mElementCount < mElements.Count;
		Invalidated?.Invoke(this, EventArgs.Empty);
		return true;
	}

	public SKBitmap CreateBitmap()
	{
		var src = Image;
		var info = src.Info;
		var bitmap = new SKBitmap(info, src.RowBytes);

		using var canvas = new SKCanvas(bitmap);

		canvas.DrawBitmap(src, SKPoint.Empty);

		for (var i = 0; i < mElementCount; i++)
			mElements[i].Draw(canvas, info);

		return bitmap;
	}

	public void Save(Stream stream)
	{
		using var bitmap = CreateBitmap();
		using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);

		data.SaveTo(stream);
	}
}
