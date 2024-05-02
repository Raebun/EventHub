using EventHub.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace EventHub.Services;

public class OrderService : IOrderService
{
	readonly HttpClient _httpClient;
	readonly JsonSerializerOptions _serializerOptions;

	public OrderService()
	{
		_httpClient = new HttpClient(Constants.HttpClientHandler);
		_serializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};
	}

	public async Task<bool> BuyTicket(int eventId, string userId, string firstName, string lastName)
	{
		try
		{
			Uri uri = new(string.Format(Constants.RestUrl, string.Empty));

			var ticketData = new
			{
				firstname = firstName,
				lastname = lastName
			};

			var ticketJson = JsonSerializer.Serialize(ticketData, _serializerOptions);
			var httpContent = new StringContent(ticketJson, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(uri + $"Ticket/purchase/{eventId}/{userId}", httpContent);

			return response.IsSuccessStatusCode;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error buying ticket: {ex.Message}");
			return false;
		}
	}
}