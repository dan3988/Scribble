using System.Runtime.CompilerServices;

using Scribble.Media;

namespace Scribble.Helpers;

/// <summary>
/// Listens for the <c>SourceUpdated</c> event on it's icon and forces the toolbar to update.
/// </summary>
public sealed class FixedToolbarItem : ToolbarItem
{
	private ImageSource mIcon;
	private bool mIconChanging;

	public FixedToolbarItem()
	{
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();

		if (mIcon != null && !mIconChanging)
		{
			try
			{
				mIconChanging = true;
				SetInheritedBindingContext(mIcon, BindingContext);
			}
			finally
			{
				mIconChanging = false;
			}
		}
	}

	protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		base.OnPropertyChanged(propertyName);

		if (propertyName == nameof(IconImageSource) && !mIconChanging)
		{
			if (mIcon != null)
			{
				mIcon.RemoveSourceChangedHandler(OnIconSourceChanged);
				SetInheritedBindingContext(mIcon, null);
			}

			mIcon = IconImageSource;
			if (mIcon != null)
			{
				SetInheritedBindingContext(mIcon, BindingContext);
				mIcon.AddSourceChangedHandler(OnIconSourceChanged);
			}
		}
	}

	private void OnIconSourceChanged(object sender, EventArgs e)
	{
		if (!mIconChanging)
		{
			try
			{
				mIconChanging = true;
				OnPropertyChanged(nameof(IconImageSource));
			}
			finally
			{
				mIconChanging = false;
			}
		}
	}
}
