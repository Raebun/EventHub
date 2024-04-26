using EventHub.Services;

namespace EventHub.Views;

public partial class Loading : ContentPage
{
	private readonly AuthService _authService;

	public Loading()
	{
		InitializeComponent();
		_authService = new AuthService();
	}

	protected async override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		if (await _authService.IsAuthenticatedAsync())
		{
			// User is logged in, redirect to Home page
			await Shell.Current.GoToAsync("//Home");
		}
		else
		{
			// User is not logged in, redirect to Login page
			await Shell.Current.Navigation.PushAsync(new Login());
		}
	}
}