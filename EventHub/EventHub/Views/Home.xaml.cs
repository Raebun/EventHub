using EventHub.ViewModels;

namespace EventHub.Views;

public partial class Home : ContentPage
{
	public Home()
	{
		InitializeComponent();
		InitializeAsync();

		var httpClient = new HttpClient(Constants.HttpClientHandler);
		BindingContext = new HomeViewModel(httpClient);

		if (BindingContext is HomeViewModel viewModel)
		{
			_ = viewModel.UpdateUserInfoAsync();
		}
	}

	private async void InitializeAsync()
	{
		await GetAuthTokenAsync();
	}

	private async Task GetAuthTokenAsync()
	{
		try
		{
			// Retrieve the stored token from SecureStorage
			string authToken = await SecureStorage.GetAsync("auth_token");

			// Check if the token exists
			if (!string.IsNullOrEmpty(authToken))
			{
				// Token exists, you can use it for authentication or other purposes
				Console.WriteLine($"Authentication Token: {authToken}");
			}
			else
			{
				// Token does not exist or could not be retrieved
				Console.WriteLine("Authentication token not found.");
			}
		}
		catch (Exception ex)
		{
			// Handle any exceptions, such as KeyNotFoundException or PlatformNotSupportedException
			Console.WriteLine($"Error retrieving auth token: {ex.Message}");
		}
	}
}