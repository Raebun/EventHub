using Shared.Entities;
using Shared.Models;

namespace Api.Services.Interfaces;

public interface IReviewService
{
    Task<List<Review>> GetReviewsByEventIdAsync(int eventId);
    Task<Review> AddReviewAsync(int eventId, ReviewCreateModel review);
    Task<Review> AddResponseToReviewAsync(int reviewId, string response);
}