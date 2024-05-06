using Api.Services.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Models;

namespace Api.Services
{
	public class TicketService : ITicketService
	{
		private readonly DataContext _context;

		public TicketService(DataContext context)
		{
			_context = context;
		}

		public async Task<List<Ticket>> GetTicketsForEventAsync(int eventId)
		{
			return await _context.Tickets
				.Include(t => t.User)
				.Include(t => t.Event)
				.Where(t => t.EventId == eventId)
				.ToListAsync();
		}

        public async Task<List<Ticket>> GetTicketsForUserAsync(Guid userId)
		{
			return await _context.Tickets
				.Include(t => t.User)
				.Include(t => t.Event)
				.Where(t => t.UserId == userId)
				.ToListAsync();
		}

		public async Task<Ticket> PurchaseTicketAsync(TicketCreateModel model, int eventId, Guid userId)
		{
			if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName))
			{
				throw new ArgumentException("First name and last name are required fields.");
			}

			var existingEvent = await _context.Events.FindAsync(eventId);
			if (existingEvent == null)
			{
				throw new ArgumentException($"Event with ID {eventId} does not exist.");
			}

			var userIdString = userId.ToString();
			var existingUser = await _context.Users.FindAsync(userIdString);
			if (existingUser == null)
			{
				throw new ArgumentException($"User with ID {userId} does not exist.");
			}

			var ticket = new Ticket
			{
				EventId = eventId,
				UserId = userId,
				FirstName = model.FirstName,
				LastName = model.LastName,
				PurchaseDate = DateTime.Now
			};

			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					_context.Tickets.Add(ticket);
					await _context.SaveChangesAsync();

					await transaction.CommitAsync();
				}
				catch (DbUpdateException ex)
				{
					await transaction.RollbackAsync();
					throw new Exception("Error occurred while purchasing ticket.", ex);
				}
			}

			return ticket;
		}
	}
}
