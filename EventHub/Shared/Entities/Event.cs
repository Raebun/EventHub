namespace Shared.Entities
{
	public class Event
	{
		public int EventId { get; set; }
		public required string EventName { get; set; }
		public required string EventDescription { get; set; }
		public required DateTime EventDate { get; set; }
		public required string Location { get; set; }
		public required float TicketPrice { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
		public User? User { get; set; }
	}
}
