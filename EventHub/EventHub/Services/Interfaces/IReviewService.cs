using Shared.Models;

namespace EventHub.Services.Interfaces;

public interface IReviewService
{
    Task<List<ReviewModel>> LoadReviewsAsync(int eventId);
    Task<bool> SendReviewAsync(int eventId, float rating, string reviewText);
}