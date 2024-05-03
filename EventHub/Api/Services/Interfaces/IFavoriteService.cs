using Shared.Entities;

namespace Api.Services.Interfaces;

public interface IFavoriteService
{
	Task<List<Favorite>> GetFavoritesForUserAsync(Guid userId);
	Task AddFavoriteAsync(Guid userId, int eventId);
	Task RemoveFavoriteAsync(Guid userId, int eventId);
}