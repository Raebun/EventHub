using Api.Services.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Api.Services;

public class FavoriteService : IFavoriteService
{
	private readonly DataContext _context;

	public FavoriteService(DataContext context)
	{
		_context = context;
	}

	public async Task<List<Favorite>> GetFavoritesForUserAsync(Guid userId)
	{
		return await _context.Favorites
			.Include(f => f.User)
			.Include(f => f.Event)
			.Where(f => f.UserId == userId)
			.ToListAsync();
	}

	public async Task AddFavoriteAsync(Guid userId, int eventId)
	{
		var existingEvent = await _context.Events.FindAsync(eventId);
		if (existingEvent == null)
		{
			throw new Exception("Event does not exist.");
		}

		var existingFavorite = await _context.Favorites
			.FirstOrDefaultAsync(f => f.UserId == userId && f.EventId == eventId);

		if (existingFavorite != null)
		{
			throw new Exception("Event is already marked as favorite by the user.");
		}

		var favorite = new Favorite
		{
			UserId = userId,
			EventId = eventId
		};

		_context.Favorites.Add(favorite);
		await _context.SaveChangesAsync();
	}

	public async Task RemoveFavoriteAsync(Guid userId, int eventId)
	{
		var existingFavorite = await _context.Favorites
			.FirstOrDefaultAsync(f => f.UserId == userId && f.EventId == eventId);

		if (existingFavorite == null)
		{
			throw new Exception("Event is not marked as favorite by the user.");
		}

		_context.Favorites.Remove(existingFavorite);
		await _context.SaveChangesAsync();
	}
}