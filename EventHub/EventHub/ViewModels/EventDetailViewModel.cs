using EventHub.Models;
using EventHub.Services;
using EventHub.Services.Interfaces;
using EventHub.Views;
using System.Windows.Input;

namespace EventHub.ViewModels;

[QueryProperty(nameof(EventItem), "Events")]
public class EventDetailViewModel : BaseViewModel
{
	private readonly MessagingService _messagingService;

	private readonly IFavoriteService _favoriteService;
	private Events _eventItem;
	public ICommand BookNowCommand { get; private set; }
	public ICommand AddToFavoritesCommand { get; private set; }

	public EventDetailViewModel(IFavoriteService favoriteService, MessagingService messagingService) 
	{
		_favoriteService = favoriteService;
		_messagingService = messagingService;
		BookNowCommand = new Command(async () => await SelectionChanged(_eventItem));
		AddToFavoritesCommand = new Command(async () => await AddToFavorites(_eventItem));
	}

	public Events EventItem
	{
		get => _eventItem;
		set
		{
			_eventItem = value;
			OnPropertyChanged();
		}
	}

	private async Task SelectionChanged(Events eventItem)
	{
		if (eventItem == null) return;

		var navigationParameter = new Dictionary<string, object>
		{
			{ nameof(Events), eventItem }
		};
		await Shell.Current.GoToAsync(nameof(Order), navigationParameter);
	}

	private async Task AddToFavorites(Events eventItem)
	{
		try
		{
			string userId = await SecureStorage.GetAsync("user_id");
			bool addToFavoritesSuccessful = await _favoriteService.AddToFavoritesAsync(userId, eventItem.EventId);

			if (addToFavoritesSuccessful)
			{
				await Application.Current.MainPage.DisplayAlert("Success", "Event added to favorites.", "OK");
				_messagingService.NotifyFavoritesUpdated();
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Failed", "Failed to add event to favorites.", "OK");
			}
		}
		catch
		{
			await Application.Current.MainPage.DisplayAlert("Failed", "Failed to add event to favorites.", "OK");
		}
	}
}
