using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Scribble.Helpers;

public abstract class AbstractViewModel : IViewModel
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected AbstractViewModel()
	{
	}

	protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
		=> PropertyChanged?.Invoke(this, new(propertyName));


	void IViewModel.OnPropertyChanged(string propertyName)
		=> OnPropertyChanged(propertyName);
}
