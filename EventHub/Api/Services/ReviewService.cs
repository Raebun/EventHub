using Api.Services.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Models;

namespace Api.Services;

public class ReviewService : IReviewService
{
    private readonly DataContext _context;

    public ReviewService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetReviewsByEventIdAsync(int eventId)
    {
        return await _context.Reviews
            .Where(r => r.EventId == eventId)
            .ToListAsync();
    }

    public async Task<Review> AddReviewAsync(int eventId, ReviewCreateModel review)
    {
        var newReview = new Review
        {
            EventId = eventId,
            UserId = review.UserId,
            Rating = review.Rating,
            ReviewText = review.ReviewText,
            ReviewDate = DateTime.UtcNow
        };

        _context.Reviews.Add(newReview);
        await _context.SaveChangesAsync();
        return newReview;
    }

    public async Task<Review> AddResponseToReviewAsync(int reviewId, string response)
    {
        var review = await _context.Reviews.FindAsync(reviewId);
        if (review == null)
        {
            throw new Exception("Review not found");
        }

        try
        {
            review.Response = response;
            await _context.SaveChangesAsync();
            return review;
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding response to review", ex);
        }
    }
}
