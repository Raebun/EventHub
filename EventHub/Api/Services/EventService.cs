using Api.Services.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Models;

namespace Api.Services
{
	/// <summary>
	/// Service for event-related operations.
	/// </summary>
	public class EventService : IEventService
	{
		private readonly DataContext _context;

		public EventService(DataContext context)
		{
			_context = context;
		}

		/// <inheritdoc />
		public async Task<List<Event>> GetEventsAsync()
		{
			return await _context.Events.ToListAsync();
		}

		/// <inheritdoc />
		public async Task<Event> GetEventByIdAsync(int id)
		{
			return await _context.Events.FindAsync(id);
		}

		/// <inheritdoc />
		public async Task<Event> CreateEventAsync(EventCreateModel model)
		{
			var newEvent = new Event
			{
				EventName = model.EventName,
				EventDescription = model.EventDescription,
				EventDate = model.EventDate,
				Location = model.Location,
				TicketPrice = model.TicketPrice,
				UserId = model.UserId
			};

			_context.Events.Add(newEvent);
			await _context.SaveChangesAsync();

			return newEvent;
		}
	}
}
