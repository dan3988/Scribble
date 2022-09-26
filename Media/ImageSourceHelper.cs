using System.Diagnostics;
using System.Reflection;

namespace Scribble.Media;

public static class ImageSourceHelper
{
	private static readonly Action<ImageSource, EventHandler> mSourceChangedAdd;
	private static readonly Action<ImageSource, EventHandler> mSourceChangedRemove;

	static ImageSourceHelper()
	{
		const BindingFlags eventFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;

		var handler = typeof(ImageSource).GetEvent("SourceChanged", eventFlags);
		if (handler == null || handler.EventHandlerType != typeof(EventHandler))
		{
			Debug.Assert(false, "event System.EventHandler SourceChanged not found on Xamarin.Forms.ImageSource");
		}
		else
		{
			mSourceChangedAdd = (Action<ImageSource, EventHandler>)Delegate.CreateDelegate(typeof(Action<ImageSource, EventHandler>), handler.AddMethod);
			mSourceChangedRemove = (Action<ImageSource, EventHandler>)Delegate.CreateDelegate(typeof(Action<ImageSource, EventHandler>), handler.RemoveMethod);
		}
	}

	public static void AddSourceChangedHandler(this ImageSource source, EventHandler handler)
		=> mSourceChangedAdd.Invoke(source, handler);

	public static void RemoveSourceChangedHandler(this ImageSource source, EventHandler handler)
		=> mSourceChangedRemove.Invoke(source, handler);
}
