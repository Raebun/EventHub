using EventHub.Services.Interfaces;
using Shared.Models;
using System.Text;
using System.Text.Json;

namespace EventHub.Services;

public class ReviewService : IReviewService
{
    readonly HttpClient _httpClient;
    readonly JsonSerializerOptions _serializerOptions;
    public List<ReviewModel> ReviewItems { get; private set; }

    public ReviewService()
    {
        _httpClient = new HttpClient(Constants.HttpClientHandler);
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<List<ReviewModel>> LoadReviewsAsync(int eventId)
    {
        ReviewItems = [];

        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
        try
        {
            var response = await _httpClient.GetAsync(uri + $"Review/event/{eventId}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                ReviewItems = JsonSerializer.Deserialize<List<ReviewModel>>(content, _serializerOptions);
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

        return ReviewItems;
    }

    public async Task<bool> SendReviewAsync(int eventId, float rating, string reviewText)
    {
        try
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));
            string userId = await SecureStorage.GetAsync("user_id");

            var addReview = new
            {
                EventId = eventId,
                UserId = userId,
                Rating = rating,
                ReviewText = reviewText
            };

            var json = JsonSerializer.Serialize(addReview);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri + $"Review/event/{eventId}", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to add review to event: {response.StatusCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding review to event: {ex.Message}");
            return false;
        }
    }
}