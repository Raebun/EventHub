namespace EventHub.Models;

public class Ticket
{
	public int TicketId { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime PurchaseDate { get; set; }
	public Events Event { get; set; }
}
