using System.Runtime.CompilerServices;

namespace Scribble.Helpers;

public static class ViewModelExtensions
{
	public static void SetPropertyValueRef<T>(this IViewModel self, ref T field, T value, [CallerMemberName] string propertyName = null!)
		where T : class
	{
		if (!ReferenceEquals(field, value))
		{
			field = value;
			self.OnPropertyChanged(propertyName);
		}
	}

	public static void SetPropertyValueEqu<T>(this IViewModel self, ref T field, T value, [CallerMemberName] string propertyName = null!)
		where T : IEquatable<T>
	{
		if (field is null ? value is null : !field.Equals(value))
		{
			field = value;
			self.OnPropertyChanged(propertyName);
		}
	}

	public static void SetPropertyValueBln(this IViewModel self, ref bool field, bool value, [CallerMemberName] string propertyName = null!)
	{
		if (field != value)
		{
			field = value;
			self.OnPropertyChanged(propertyName);
		}
	}

	public static void SetPropertyValue<T>(this IViewModel self, ref T field, T value, [CallerMemberName] string propertyName = null!)
		where T : IEqualityOperators<T, T>
	{
		if (field != value)
		{
			field = value;
			self.OnPropertyChanged(propertyName);
		}
	}
}
