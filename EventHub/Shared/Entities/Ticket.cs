namespace Shared.Entities
{
	public class Ticket
	{
		public int TicketId { get; set; }
		public int EventId { get; set; }
		public Guid UserId { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public DateTime PurchaseDate { get; set; }
		public User? User { get; set; }
		public Event? Event { get; set; }
	}
}
