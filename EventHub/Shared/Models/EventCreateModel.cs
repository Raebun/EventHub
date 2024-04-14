namespace Shared.Models
{
	public class EventCreateModel
	{
		public required string EventName { get; set; }

		public required string EventDescription { get; set; }

		public DateTime EventDate { get; set; }

		public required string Location { get; set; }

		public required float TicketPrice { get; set; }

		public Guid UserId { get; set; }
	}
}
