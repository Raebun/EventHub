using EventHub.ViewModels;

namespace EventHub.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();

		var httpClient = new HttpClient(Constants.HttpClientHandler);
		BindingContext = new RegisterViewModel(httpClient);

		if (BindingContext is RegisterViewModel viewModel)
		{
			viewModel.RegisterSuccess += OnRegistrationSuccess;
		}
	}

	private async void OnRegistrationSuccess(object sender, EventArgs e)
	{
		await DisplayAlert("Success", "You've successfully registered", "OK");
		await Shell.Current.Navigation.PushAsync(new Login());
	}

	private async void BackButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.Navigation.PopAsync();
	}
}
