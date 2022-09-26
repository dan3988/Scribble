using System.Globalization;

namespace Scribble.Helpers;

public abstract class ValueConverter : IValueConverter
{
	private sealed class DelegateConverter<TTarget, TSource> : ValueConverter<TTarget, TSource>
	{
		private readonly Func<TSource, TTarget> mConvertTo;
		private readonly Func<TTarget, TSource> mConvertBack;

		public DelegateConverter(Func<TSource, TTarget> convertTo, Func<TTarget, TSource> convertBack)
		{
			mConvertTo = convertTo;
			mConvertBack = convertBack;
		}

		protected override TTarget ConvertTo(TSource value)
			=> mConvertTo.Invoke(value);

		protected override TSource ConvertBack(TTarget value)
			=> mConvertBack == null ? base.ConvertBack(value) : mConvertBack.Invoke(value);
	}

	private sealed class DelegateConverter<TTarget, TSource, TParameter> : ValueConverter<TTarget, TSource, TParameter>
	{
		private readonly Func<TSource, TParameter, TTarget> mConvertTo;
		private readonly Func<TTarget, TParameter, TSource> mConvertBack;

		public DelegateConverter(Func<TSource, TParameter, TTarget> convertTo, Func<TTarget, TParameter, TSource> convertBack)
		{
			mConvertTo = convertTo;
			mConvertBack = convertBack;
		}

		protected override TTarget ConvertTo(TSource value, TParameter parameter)
			=> mConvertTo.Invoke(value, parameter);

		protected override TSource ConvertBack(TTarget value, TParameter parameter)
			=> mConvertBack == null ? base.ConvertBack(value, parameter) : mConvertBack.Invoke(value, parameter);
	}

	public static ValueConverter<TTarget, TSource> Create<TTarget, TSource>(Func<TSource, TTarget> convertTo, Func<TTarget, TSource> convertBack = null)
		=> new DelegateConverter<TTarget, TSource>(convertTo, convertBack);

	public static ValueConverter<TTarget, TSource, TParameter> Create<TTarget, TSource, TParameter>(Func<TSource, TParameter, TTarget> convertTo, Func<TTarget, TParameter, TSource> convertBack = null)
		=> new DelegateConverter<TTarget, TSource, TParameter>(convertTo, convertBack);

	protected abstract object ConvertTo(object value, Type targetType, object parameter, CultureInfo culture);

	protected virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotSupportedException(GetType().Name + " only supports converting to the target type");

	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> ConvertTo(value, targetType, parameter, culture);

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> ConvertBack(value, targetType, parameter, culture);
}

public abstract class ValueConverter<TTarget, TSource> : ValueConverter
{
	protected virtual T ConvertValue<T>(object value, CultureInfo culture)
		=> (T)Convert.ChangeType(value, typeof(T), culture);

	protected abstract TTarget ConvertTo(TSource value);

	protected virtual TSource ConvertBack(TTarget value)
		=> throw new NotSupportedException(GetType().Name + " only supports converting to the target type");

	protected sealed override object ConvertTo(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var source = ConvertValue<TSource>(value, culture);
		return ConvertTo(source);
	}

	protected sealed override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var target = ConvertValue<TTarget>(value, culture);
		return ConvertBack(target);
	}
}

public abstract class ValueConverter<TTarget, TSource, TParam> : ValueConverter
{
	protected virtual T ConvertValue<T>(object value, CultureInfo culture)
		=> (T)Convert.ChangeType(value, typeof(T), culture);

	protected abstract TTarget ConvertTo(TSource value, TParam parameter);

	protected virtual TSource ConvertBack(TTarget value, TParam parameter)
		=> throw new NotSupportedException(GetType().Name + " only supports converting to the target type");

	protected sealed override object ConvertTo(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var source = ConvertValue<TSource>(value, culture);
		var param = ConvertValue<TParam>(value, culture);
		return ConvertTo(source, param);
	}

	protected sealed override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var target = ConvertValue<TTarget>(value, culture);
		var param = ConvertValue<TParam>(value, culture);
		return ConvertBack(target, param);
	}
}