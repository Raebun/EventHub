namespace Shared.Models;

public class TicketModel
{
    public int TicketId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime PurchaseDate { get; set; }
    public Events Event { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}