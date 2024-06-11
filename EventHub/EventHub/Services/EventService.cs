using EventHub.Models;
using EventHub.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EventHub.Services
{
	public class EventService : IEventService
	{
		readonly HttpClient _httpClient;
		readonly JsonSerializerOptions _serializerOptions;
		public List<Events> EventItems { get; private set; }

		public EventService()
		{
			_httpClient= new HttpClient(Constants.HttpClientHandler);
			_serializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true
			};
		}

		public async Task<List<Events>> LoadEventsAsync()
		{
			EventItems = [];

            Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
			try
			{
				var response = await _httpClient.GetAsync(uri + "Event");

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					EventItems = JsonSerializer.Deserialize<List<Events>>(content, _serializerOptions);
				}
				else
				{
					Console.WriteLine($"Failed to retrieve events: {response.StatusCode}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error retrieving events: {ex.Message}");
			}

			return EventItems;
		}

		public async Task<UserInfo> UpdateUserInfoAsync()
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
	}
}
