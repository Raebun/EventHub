using Shared.Entities;

namespace Shared.Models;

public class ReviewModel
{
    public int ReviewId { get; set; }
    public float Rating { get; set; }
    public string? ReviewText { get; set; }
    public string? Response { get; set; }
    public DateTime ReviewDate { get; set; }
    public Event? Event { get; set; }
}