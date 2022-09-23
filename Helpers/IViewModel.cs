using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Scribble.Helpers;

public interface IViewModel : INotifyPropertyChanged
{
	void OnPropertyChanged(string propertyName);
}
