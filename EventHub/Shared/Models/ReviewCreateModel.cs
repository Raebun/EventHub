namespace Shared.Models;

public class ReviewCreateModel
{
    public int EventId { get; set; }
    public Guid UserId { get; set; }
    public float Rating { get; set; }
    public string? ReviewText { get; set; }
    public DateTime ReviewDate { get; set; }
}