using Scribble.ViewModels;

namespace Scribble;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new MainPage
		{
			BindingContext = new MainViewModel()
		};
	}
}
