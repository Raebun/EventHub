namespace EventHub.Services;

public class MessagingService
{
	public event EventHandler FavoritesUpdated;

	public void NotifyFavoritesUpdated()
	{
		FavoritesUpdated?.Invoke(this, EventArgs.Empty);
	}
}