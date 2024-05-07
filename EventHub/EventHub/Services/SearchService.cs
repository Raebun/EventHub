using EventHub.Services.Interfaces;
using System.Text.Json;
using EventHub.Models;

namespace EventHub.Services;

public class SearchService : ISearchService
{
    readonly HttpClient _httpClient;
    readonly JsonSerializerOptions _serializerOptions;

    public SearchService()
    {
        _httpClient = new HttpClient(Constants.HttpClientHandler);
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
    public async Task<List<Events>> SortEventsBy(string sortBy)
    {
        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
        var response = await _httpClient.GetAsync(uri + $"Search/sort?sortBy={sortBy}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var events = JsonSerializer.Deserialize<List<Events>>(content, _serializerOptions);
            return events;
        }
        else
        {
            throw new HttpRequestException($"Failed to retrieve events: {response.StatusCode}");
        }
    }
}