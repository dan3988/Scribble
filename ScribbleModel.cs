using System.Windows.Input;

using Scribble.Helpers;
using Scribble.Tools;

using SkiaSharp;

namespace Scribble;

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
		set => this.SetPropertyValueBln(ref mCanUndo, value);
	}

	private bool mCanRedo;
	public bool CanRedo
	{
		get => mCanRedo;
		set => this.SetPropertyValueBln(ref mCanRedo, value);
	}

	private readonly List<IScribbleElement> mElements;
	private int mElementCount;

	public IReadOnlyList<IScribbleElement> Elements { get; }

	public ICommand UndoCommand { get; }

	public ICommand RedoCommand { get; }

	public event EventHandler Invalidated;

	public ScribbleModel()
	{
		mElements = new();
		Elements = new ElementList(this);
		UndoCommand = new Command(() => Undo());
		RedoCommand = new Command(() => Redo());
	}

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
}
