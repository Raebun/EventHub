using EventHub.Models;
using EventHub.Services.Interfaces;
using System.Text.Json;

namespace EventHub.Services;

public class TicketService : ITicketService
{
	public List<Ticket> TicketItems { get; private set; }

	readonly HttpClient _httpClient;
	readonly JsonSerializerOptions _serializerOptions;

	public TicketService()
	{
		_httpClient = new HttpClient(Constants.HttpClientHandler);
		_serializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};
	}

	public async Task<List<Ticket>> LoadTicketsAsync(string userId)
	{
		TicketItems = [];

		Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
		try
		{
			var response = await _httpClient.GetAsync(uri + $"Ticket/user/{userId}");

			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();
				TicketItems = JsonSerializer.Deserialize<List<Ticket>>(content, _serializerOptions);
			}
			else
			{
				Console.WriteLine($"Failed to retrieve tickets: {response.StatusCode}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error retrieving tickets: {ex.Message}");
		}

		return TicketItems;
	}
}