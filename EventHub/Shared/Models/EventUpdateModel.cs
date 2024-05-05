namespace Shared.Models
{
	public class EventUpdateModel
	{
		public required string EventName { get; set; }
		public required string EventDescription { get; set; }
		public DateTime? EventDate { get; set; }
		public required string Location { get; set; }
		public required bool IsActive { get; set; }
		public float? TicketPrice { get; set; }
	}
}
