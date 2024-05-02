using EventHub.ViewModels;

namespace EventHub.Views;

public partial class Profile : ContentPage
{
	public Profile(UserViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}