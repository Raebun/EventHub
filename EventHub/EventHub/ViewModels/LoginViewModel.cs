using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;
using System.Text;
using System.Windows.Input;
using System.Net.Http.Headers;

namespace EventHub.ViewModels
{
	public partial class LoginViewModel : ObservableObject
	{
		private string _email;
		private string _password;
		private string _errorMessage;

		public ICommand SignInCommand { get; }
		public event EventHandler LoginSuccess;
		private readonly HttpClient _httpClient;

		public LoginViewModel(HttpClient httpClient)
		{
			_httpClient = httpClient;
			SignInCommand = new Command(async () => await SignInAsync());
		}

		public string Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}

		public string Password
		{
			get => _password;
			set => SetProperty(ref _password, value);
		}

		public string ErrorMessage
		{
			get => _errorMessage;
			set => SetProperty(ref _errorMessage, value);
		}

		public async Task SignInAsync()
		{
			var loginModel = new { email = Email, Password };
			var json = JsonSerializer.Serialize(loginModel);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
			var response = await _httpClient.PostAsync(uri + "login", content);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

				await SecureStorage.SetAsync("auth_token", tokenResponse.accessToken);
				string authToken = await SecureStorage.GetAsync("auth_token");

				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

				var userInfoResponse = await _httpClient.GetAsync(uri + "User/me");
				if (userInfoResponse.IsSuccessStatusCode)
				{
					var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
					var userInfo = JsonSerializer.Deserialize<UserInfo>(userInfoContent);

					await SecureStorage.SetAsync("user_id", userInfo.id);
				}

				LoginSuccess?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				HandleLoginFailure();
			}
		}

		private void HandleLoginFailure()
		{
			ErrorMessage = "Login failed. Please check your credentials and try again.";
		}

		public class TokenResponse
		{
			public string tokenType { get; set; }
			public string accessToken { get; set; }
			public int expiresIn { get; set; }
			public string refreshToken { get; set; }
		}

		public class UserInfo
		{
			public string id { get; set; }
		}
	}
}
