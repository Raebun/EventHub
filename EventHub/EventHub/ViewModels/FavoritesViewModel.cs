using CommunityToolkit.Mvvm.ComponentModel;
using EventHub.Models;
using EventHub.Services;
using EventHub.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EventHub.ViewModels;

public class FavoritesViewModel : ObservableObject
{
	private readonly MessagingService _messagingService;
	private readonly IFavoriteService _favoriteService;
	public ObservableCollection<Favorites> FavoriteItems { get; set; } = [];
	public ICommand DeleteCommand { get; }

	public FavoritesViewModel(IFavoriteService favoriteService, MessagingService messagingService)
	{
		_messagingService = messagingService;
		_messagingService.FavoritesUpdated += (sender, args) =>
		{
			LoadFavoritesAsync();
		};
		_favoriteService = favoriteService;
		DeleteCommand = new Command<Favorites>(async (favorite) => await DeleteFavoriteAsync(favorite));
		_ = LoadFavoritesAsync();
	}

	public async Task LoadFavoritesAsync()
	{
		FavoriteItems.Clear();
		string userId = await SecureStorage.GetAsync("user_id");
		var tasks = await _favoriteService.GetFavoritesAsync(userId);
		tasks.ForEach(FavoriteItems.Add);
	}

	private async Task DeleteFavoriteAsync(Favorites favorite)
	{
		try
		{
			string userId = await SecureStorage.GetAsync("user_id");
			int eventId = favorite.Event.EventId;

			bool deletionSuccessful = await _favoriteService.DeleteFavoriteAsync(userId, eventId);

			if (deletionSuccessful)
			{
				FavoriteItems.Remove(favorite);
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Failed", "Failed to delete favorite event.", "OK");
			}
		}
		catch
		{
			await Application.Current.MainPage.DisplayAlert("Failed", "Failed to delete favorite event.", "OK");
		}
	}
}