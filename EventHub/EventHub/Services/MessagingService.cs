namespace EventHub.Services;

public class MessagingService
{
	public event EventHandler FavoritesUpdated;
	public event EventHandler ProfileUpdated;

	public void NotifyFavoritesUpdated()
	{
		FavoritesUpdated?.Invoke(this, EventArgs.Empty);
	}

	public void NotifyProfileUpdated()
	{
		ProfileUpdated?.Invoke(this, EventArgs.Empty);
	}
}