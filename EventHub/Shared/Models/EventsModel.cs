namespace Shared.Models;

public class Events
{
    public int EventId { get; set; }
    public string EventName { get; set; }
    public string EventDescription { get; set; }
    public string Location { get; set; }
    public float TicketPrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime EventDate { get; set; }
    public int TicketsSold { get; set; }
}