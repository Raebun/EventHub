using Shared.Entities;
using Shared.Models;

namespace Api.Services.Interfaces
{
	public interface ITicketService
	{
		Task<List<Ticket>> GetTicketsForEventAsync(int eventId);
		Task<List<Ticket>> GetTicketsForUserAsync(Guid userId);
		Task<Ticket> PurchaseTicketAsync(TicketCreateModel model, int eventId, Guid userId);
	}
}
