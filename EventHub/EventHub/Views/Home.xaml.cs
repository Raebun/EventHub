using EventHub.ViewModels;

namespace EventHub.Views;

public partial class Home : ContentPage
{
	public Home(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		BindingContext = homeViewModel;
	}
}