using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EventHub.ViewModels
{
	public partial class HomeViewModel : ObservableObject
	{
		private string? _fullName;

		private readonly HttpClient _httpClient;

		public string FullName
		{
			get { return _fullName; }
			set { SetProperty(ref _fullName, value); }
		}

		public HomeViewModel(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task UpdateUserInfoAsync()
		{
			try
			{
				string authToken = await SecureStorage.GetAsync("auth_token");
				if (!string.IsNullOrEmpty(authToken))
				{
					_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

					Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
					var response = await _httpClient.GetAsync(uri + "User/me");

					if (response.IsSuccessStatusCode)
					{
						string json = await response.Content.ReadAsStringAsync();

						// Deserialize the JSON into a .NET object using System.Text.Json
						var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
						UserInfo userInfo = JsonSerializer.Deserialize<UserInfo>(json, options);

						// Update the UI with the user's name
						FullName = $"{userInfo.FirstName} {userInfo.LastName}";
					}
					else
					{
						Console.WriteLine($"Failed to retrieve user information: {response.StatusCode}");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error retrieving user information: {ex.Message}");
			}
		}

		public class UserInfo
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}
	}
		
}
