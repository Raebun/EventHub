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

		public async Task<bool> EventExistsAsync(int id)
		{
			// Check if there's any event with the given ID
			return await _context.Events.AnyAsync(e => e.EventId == id);
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

		/// <inheritdoc />
		public async Task<Event> UpdateEventAsync(int id, EventUpdateModel model)
		{
			var existingEvent = await _context.Events.FindAsync(id);

			if (existingEvent == null)
				throw new Exception("Event not found");
			
			existingEvent.EventName = model.EventName;
			existingEvent.EventDescription = model.EventDescription;
			existingEvent.EventDate = model.EventDate ?? existingEvent.EventDate;
			existingEvent.Location = model.Location;
			existingEvent.TicketPrice = model.TicketPrice ?? existingEvent.TicketPrice;

			await _context.SaveChangesAsync();

			return existingEvent;
		}

		/// <inheritdoc />
		public async Task<bool> DeleteEventAsync(int id)
		{
			var existingEvent = await _context.Events.FindAsync(id);

			if (existingEvent == null)
				return false;

			_context.Events.Remove(existingEvent);
			await _context.SaveChangesAsync();

			return true;
		}
	}
}
