using EventHub.Models;

namespace EventHub.Services.Interfaces;

public interface ITicketService
{
	Task<List<Ticket>> LoadTicketsAsync(string userId);
}