using Shared.Models;

namespace EventHubOrganiser.Services.Interfaces;

public interface IEventService
{
    Task<List<Events>> LoadEventsAsync();
    Task<bool> DeleteEventAsync(int eventId);
    Task<bool> AddEventAsync(EventCreateModel newEvent);
    Task<bool> UpdateEventAsync(Events updatedEvent);
    Task<int> GetTicketsSoldForEvent(int eventId);
    Task<List<TicketModel>> GetTicketHoldersAsync(int eventId);
}