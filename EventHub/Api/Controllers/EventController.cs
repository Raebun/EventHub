using Api.Services.Interfaces;
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
		public async Task<ActionResult<Event>> CreateEvent(EventCreateModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var createdEvent = await _eventService.CreateEventAsync(model);
			return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.EventId }, createdEvent);
		}
	}
}
