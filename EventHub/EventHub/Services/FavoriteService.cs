using EventHub.Models;
using EventHub.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace EventHub.Services;

public class FavoriteService : IFavoriteService
{
	readonly HttpClient _httpClient;
	readonly JsonSerializerOptions _serializerOptions;

	public List<Favorites> FavoriteItems { get; private set; }

	public FavoriteService()
	{
		_httpClient = new HttpClient(Constants.HttpClientHandler);
		_serializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};
	}

	public async Task<bool> AddToFavoritesAsync(string userId, int eventId)
	{
		try
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

			var addToFavorites = new
			{
				userId,
				eventId
			};

			var json = JsonSerializer.Serialize(addToFavorites);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(uri + "Favorite", content);

			if (response.IsSuccessStatusCode)
			{
				return true;
			}
			else
			{
				Console.WriteLine($"Failed to add event to favorites: {response.StatusCode}");
				return false;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error adding event to favorites: {ex.Message}");
			return false;
		}
	}

	public async Task<List<Favorites>> GetFavoritesAsync(string userId)
	{
		FavoriteItems = [];

		try
		{
			Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
			var response = await _httpClient.GetAsync(uri + $"Favorite/{userId}");

			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();
				FavoriteItems = JsonSerializer.Deserialize<List<Favorites>>(content, _serializerOptions);
			}
			else
			{
				Console.WriteLine($"Failed to retrieve favorites: {response.StatusCode}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error retrieving favorites: {ex.Message}");
		}

		return FavoriteItems;
	}

	public async Task<bool> DeleteFavoriteAsync(string userId, int eventId)
	{
		try
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

			var deleteFavorite = new
			{
				userId,
				eventId
			};

			var jsonContent = JsonSerializer.Serialize(deleteFavorite);
			var request = new HttpRequestMessage(HttpMethod.Delete, uri + "Favorite")
			{
				Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
			};

			var response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				return true;
			}
			else
			{
				Console.WriteLine($"Failed to delete favorite: {response.StatusCode}");
				return false;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error deleting favorite: {ex.Message}");
			return false;
		}
	}
}
