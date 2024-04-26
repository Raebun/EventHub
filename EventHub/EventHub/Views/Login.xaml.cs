using EventHub.ViewModels;

namespace EventHub.Views;

public partial class Login : ContentPage
{
	int count = 0;

	public Login()
	{
		InitializeComponent();

		var httpClient = new HttpClient(Constants.HttpClientHandler);
		BindingContext = new LoginViewModel(httpClient);

		if (BindingContext is LoginViewModel viewModel)
		{
			viewModel.LoginSuccess += OnLoginSuccess;
		}
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		if (BindingContext is ViewModels.LoginViewModel viewModel)
		{
			viewModel.LoginSuccess -= OnLoginSuccess;
		}
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// Check if the user is already authenticated
		var authToken = await SecureStorage.GetAsync("auth_token");
		if (!string.IsNullOrEmpty(authToken))
		{
			await Navigation.PushAsync(new Home());
		}
	}

	private async void OnLoginSuccess(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//Home");
	}

	private async void SignUpTapped(object sender, EventArgs e)
	{
		await Shell.Current.Navigation.PushAsync(new Register());
	}

	private void BtnClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			loginBtn.Text = $"Clicked {count} time";
		else
			loginBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(loginBtn.Text);
	}
}