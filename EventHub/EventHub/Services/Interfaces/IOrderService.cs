namespace EventHub.Services.Interfaces;

public interface IOrderService
{
	Task<bool> BuyTicket(int eventId, string userId, string firstName, string lastName);
}
