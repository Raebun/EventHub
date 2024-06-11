using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Models;

namespace Api.Controllers
{
    [Route("[controller]")]
	[ApiController]
	public class EventController : Controller
	{
		private readonly IEventService _eventService;

		public EventController(IEventService eventService)
		{
			_eventService = eventService;
		}

		/// <summary>
		/// Retrieves all events.
		/// </summary>
		/// <returns>The list of events.</returns>
		[HttpGet]
		public async Task<ActionResult<List<User>>> GetEvents()
		{
			var events = await _eventService.GetEventsAsync();
			return Ok(events);
		}

		/// <summary>
		/// Retrieves an event by ID.
		/// </summary>
		/// <param name="id">The ID of the event.</param>
		/// <returns>The event object if found, otherwise returns NotFoundResult.</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<Event>> GetEvent(int id)
		{
			var eventItem = await _eventService.GetEventByIdAsync(id);
			if (eventItem == null)
				return NotFound("Event not found");

			return Ok(eventItem);
		}

		/// <summary>
		/// Creates a new event.
		/// </summary>
		/// <param name="model">The event creation model.</param>
		/// <returns>The newly created event.</returns>
		[HttpPost]
        [Authorize]
        public async Task<ActionResult<Event>> CreateEvent(EventCreateModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var createdEvent = await _eventService.CreateEventAsync(model);
			return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.EventId }, createdEvent);
		}

		/// <summary>
		/// Updates an existing event.
		/// </summary>
		/// <param name="id">The ID of the event to update.</param>
		/// <param name="model">The event update model.</param>
		/// <returns>The updated event if successful, otherwise returns NotFoundResult.</returns>
		[HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent(int id, EventUpdateModel model)
		{
			var updatedEvent = await _eventService.UpdateEventAsync(id, model);

			if (updatedEvent == null)
				return NotFound("Event not found");

			return Ok(updatedEvent);
		}

		/// <summary>
		/// Deletes an event by ID.
		/// </summary>
		/// <param name="id">The ID of the event to delete.</param>
		/// <returns>Returns a message indicating the status of the deletion.</returns>
		[HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvent(int id)
		{
			var eventExists = await _eventService.EventExistsAsync(id);
			if (!eventExists)
			{
				return NotFound($"Event with ID {id} not found");
			}

			var result = await _eventService.DeleteEventAsync(id);
			if (result)
			{
				return Ok("Event has been successfully deleted");
			}
			else
			{
				return BadRequest("Event could not be removed");
			}
		}

        /// <summary>
        /// Retrieves all events associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The list of events associated with the user.</returns>
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<List<Event>>> GetEventsByUserId(Guid userId)
        {
            var events = await _eventService.GetEventsByUserIdAsync(userId);
            return Ok(events);
        }
    }
}
