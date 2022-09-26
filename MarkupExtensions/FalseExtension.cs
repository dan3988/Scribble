using Scribble.Helpers;

namespace Scribble.MarkupExtensions;

[AcceptEmptyServiceProvider]
public sealed class FalseExtension : IMarkupExtension<bool>
{
	bool IMarkupExtension<bool>.ProvideValue(IServiceProvider serviceProvider)
		=> false;

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		=> BooleanBox.False;
}