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
		/// Checks if an event with the specified ID exists.
		/// </summary>
		/// <param name="id">The ID of the event to check.</param>
		/// <returns>True if the event exists, otherwise false.</returns>
		Task<bool> EventExistsAsync(int id);

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

		/// <summary>
		/// Updates an existing event.
		/// </summary>
		/// <param name="id">The ID of the event to update.</param>
		/// <param name="model">The event update model.</param>
		/// <returns>The updated event if successful, otherwise null.</returns>
		Task<Event> UpdateEventAsync(int id, EventUpdateModel model);

		/// <summary>
		/// Deletes an event by ID.
		/// </summary>
		/// <param name="id">The ID of the event to delete.</param>
		/// <returns>True if the event was deleted successfully, otherwise false.</returns>
		Task<bool> DeleteEventAsync(int id);

        /// <summary>
        /// Retrieves all events associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The list of events associated with the user.</returns>
        Task<List<Event>> GetEventsByUserIdAsync(Guid userId);
    }
}
