using EventHub.Models;

namespace EventHub.Services.Interfaces;

public interface IFavoriteService
{
	Task<bool> AddToFavoritesAsync(string userId, int eventId);
	Task<List<Favorites>> GetFavoritesAsync(string userId);
	Task<bool> DeleteFavoriteAsync(string userId, int eventId);
}