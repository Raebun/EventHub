using EventHubOrganiser.Services.Interfaces;
using Microsoft.JSInterop;
using Shared.Models;
using System.Text.Json;

namespace EventHubOrganiser.Services;

public class EventService : IEventService
{
    readonly HttpClient _httpClient;
    readonly JsonSerializerOptions _serializerOptions;
    private readonly IJSRuntime _js;

    public EventService(IJSRuntime js)
    {
        _js = js;
        _httpClient = new HttpClient(Constants.HttpClientHandler);
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<List<Events>> LoadEventsAsync()
    {
        var EventItems = new List<Events>();
        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
        try
        {
            var userId = await _js.InvokeAsync<string>("localStorage.getItem", "user_id");
            var response = await _httpClient.GetAsync(uri + $"Event/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                EventItems = JsonSerializer.Deserialize<List<Events>>(content, _serializerOptions);

                foreach (var ev in EventItems)
                {
                    ev.TicketsSold = await GetTicketsSoldForEvent(ev.EventId);
                }
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

    public async Task<int> GetTicketsSoldForEvent(int eventId)
    {
        try
        {
            Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
            var response = await _httpClient.GetAsync(uri + $"Ticket/event/{eventId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tickets = JsonSerializer.Deserialize<List<TicketModel>>(content, _serializerOptions);
                var ticketsSold = tickets.Count;

                return ticketsSold;
            }
            else
            {
                Console.WriteLine($"Failed to retrieve tickets sold for event {eventId}: {response.StatusCode}");
                return 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving tickets sold for event {eventId}: {ex.Message}");
            return 0;
        }
    }

    public async Task<List<TicketModel>> GetTicketHoldersAsync(int eventId)
    {
        try
        {
            Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
            var response = await _httpClient.GetAsync(uri + $"Ticket/event/{eventId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var ticketHolders = JsonSerializer.Deserialize<List<TicketModel>>(content, _serializerOptions);
                return ticketHolders;
            }
            else
            {
                Console.WriteLine($"Failed to retrieve ticket holders for event {eventId}: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving ticket holders for event {eventId}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteEventAsync(int eventId)
    {
        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
        try
        {
            var response = await _httpClient.DeleteAsync(uri + $"Event/{eventId}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to delete event: {response.StatusCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting event: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddEventAsync(EventCreateModel newEvent)
    {
        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
        try
        {
            var response = await _httpClient.PostAsJsonAsync(uri + "Event", newEvent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to add event: {response.StatusCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding event: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateEventAsync(Events updatedEvent)
    {
        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
        try
        {
            var response = await _httpClient.PutAsJsonAsync(uri + $"Event/{updatedEvent.EventId}", updatedEvent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to update event: {response.StatusCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating event: {ex.Message}");
            return false;
        }
    }
}