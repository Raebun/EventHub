namespace Shared.Entities
{
	public class Review
	{
		public int ReviewId { get; set; }
		public int EventId { get; set; }
		public Guid UserId { get; set; }
		public float Rating { get; set; }
		public string? ReviewText { get; set; }
		public DateTime ReviewDate { get; set; }
		public User? User { get; set; }
		public Event? Event { get; set; }
	}
}
