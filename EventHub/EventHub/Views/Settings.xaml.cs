using EventHub.ViewModels;

namespace EventHub.Views;

public partial class Settings : ContentPage
{
	public Settings(SettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}