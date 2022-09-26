using System.ComponentModel;
using System.Globalization;

using Microsoft.Maui.Controls.Xaml;

using Scribble.Helpers;

namespace Scribble.MarkupExtensions;

[ContentProperty(nameof(Path))]
public sealed class IfElseBindingExtension : IMarkupExtension<Binding>
{
	private sealed class Converter : ValueConverter
	{
		private readonly bool mNegate;
		private readonly object mTrue;
		private readonly object mFalse;

		public Converter(bool negate, object trueValue, object falseValue)
		{
			mNegate = negate;
			mTrue = trueValue;
			mFalse = falseValue;
		}

		protected override object ConvertTo(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var result = parameter == null ? Convert.ToBoolean(value) : Equals(value, parameter);
			return (mNegate != result) ? mTrue : mFalse;
		}
	}

	public string Path { get; set; }

	public object TrueValue { get; set; }

	public object FalseValue { get; set; }

	public bool Negate { get; set; }

	public BindingMode Mode { get; set; } = BindingMode.Default;

	public object ConverterParameter { get; set; }

	public object Source { get; set; }

	public Binding ProvideValue(IServiceProvider serviceProvider)
	{
		var provideValue = serviceProvider.GetRequiredService<IProvideValueTarget>();
		var target = (BindableProperty)provideValue.TargetProperty;
		var tc = TypeDescriptor.GetConverter(target.ReturnType);

		var trueValue = tc.ConvertFrom(TrueValue);
		var falseValue = tc.ConvertFrom(FalseValue);

		var converter = new Converter(Negate, trueValue, falseValue);
		var binding = new Binding(Path, Mode, converter, ConverterParameter, null, Source);
		return binding;
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> ProvideValue(serviceProvider);
}
