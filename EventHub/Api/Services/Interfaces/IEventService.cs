using Shared.Entities;
using Shared.Models;

namespace Api.Services.Interfaces
{
	/// <summary>
	/// Interface for event service.
	/// </summary>
	public interface IEventService
	{
		/// <summary>
		/// Retrieves all events asynchronously.
		/// </summary>
		/// <returns>The list of events.</returns>
		Task<List<Event>> GetEventsAsync();

		/// <summary>
		/// Retrieves an event by ID asynchronously.
		/// </summary>
		/// <param name="id">The ID of the event.</param>
		/// <returns>The event object if found, otherwise null.</returns>
		Task<Event> GetEventByIdAsync(int id);

		/// <summary>
		/// Creates a new event.
		/// </summary>
		/// <param name="model">The event creation model.</param>
		/// <returns>The newly created event.</returns>
		Task<Event> CreateEventAsync(EventCreateModel model);

	}
}
