using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EventHub.Models;
using EventHub.Services.Interfaces;
using Shared.Models;

namespace EventHub.Services;

public class UserService : IUserService
{
	readonly HttpClient _httpClient;
	readonly JsonSerializerOptions _serializerOptions;

	public UserService()
	{
		_httpClient = new HttpClient(Constants.HttpClientHandler);
		_serializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};
	}

	public async Task<UserInfo> GetuserInfoAsync()
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
					string content = await response.Content.ReadAsStringAsync();
					UserInfo userInfo = JsonSerializer.Deserialize<UserInfo>(content, _serializerOptions);
					return userInfo;
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
		return null;
	}

	public async Task<bool> UpdateUserInfoAsync(UserUpdate updatedUser)
	{
		try
		{
			string userId = await SecureStorage.GetAsync("user_id");
			string authToken = await SecureStorage.GetAsync("auth_token");
			if (!string.IsNullOrEmpty(authToken))
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

				Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
				var jsonUserInfo = JsonSerializer.Serialize(updatedUser, _serializerOptions);
				var content = new StringContent(jsonUserInfo, Encoding.UTF8, "application/json");

				var response = await _httpClient.PutAsync(uri + $"User/{userId}", content);

				if (response.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					Console.WriteLine($"Failed to update user information: {response.StatusCode}");
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error updating user information: {ex.Message}");
		}
		return false;
	}
}
