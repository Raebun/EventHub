using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Models;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetReviewsByEventId(int eventId)
    {
        var reviews = await _reviewService.GetReviewsByEventIdAsync(eventId);
        return Ok(reviews);
    }

    [HttpPost("event/{eventId}")]
    public async Task<IActionResult> AddReview(int eventId, ReviewCreateModel review)
    {
        try
        {
            var addedReview = await _reviewService.AddReviewAsync(eventId, review);
            return CreatedAtAction(nameof(GetReviewsByEventId), new { eventId }, addedReview);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error adding review: {ex}");
        }
    }

    [HttpPut("{reviewId}/response")]
    public async Task<IActionResult> AddResponseToReview(int reviewId, [FromBody] ReviewCreateResponseModel responseModel)
    {
        try
        {
            var updatedReview = await _reviewService.AddResponseToReviewAsync(reviewId, responseModel.Response);
            return Ok(updatedReview);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error adding response: {ex.Message}");
        }
    }

}