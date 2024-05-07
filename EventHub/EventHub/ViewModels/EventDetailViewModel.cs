using EventHub.Models;
using EventHub.Services;
using EventHub.Services.Interfaces;
using EventHub.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EventHub.ViewModels;

[QueryProperty(nameof(EventItem), "Events")]
public class EventDetailViewModel : BaseViewModel
{
	private readonly MessagingService _messagingService;
    private readonly IReviewService _reviewService;
    private readonly IFavoriteService _favoriteService;
	private Events _eventItem;
    public ObservableCollection<Shared.Models.ReviewModel> ReviewItems { get; set; } = [];

    public ICommand BookNowCommand { get; private set; }
	public ICommand AddToFavoritesCommand { get; private set; }
    public ICommand SendReviewCommand { get; private set; }

    private string _reviewText;
    public string ReviewText
    {
        get => _reviewText;
        set
        {
            _reviewText = value;
            OnPropertyChanged();
        }
    }

    private float _rating;
    public float Rating
    {
        get => _rating;
        set
        {
            _rating = value;
            OnPropertyChanged();
        }
    }

    public EventDetailViewModel(IFavoriteService favoriteService, IReviewService reviewService, MessagingService messagingService)
    {
        _favoriteService = favoriteService;
        _reviewService = reviewService;
        _messagingService = messagingService;
		BookNowCommand = new Command(async () => await SelectionChanged(_eventItem));
		AddToFavoritesCommand = new Command(async () => await AddToFavorites(_eventItem));
        SendReviewCommand = new Command(async () => await SendReviewAsync());
    }

    public Events EventItem
	{
		get => _eventItem;
		set
		{
			_eventItem = value;
			OnPropertyChanged();
            _ = LoadReviewsAsync();
        }
	}

    public async Task LoadReviewsAsync()
    {
        ReviewItems.Clear();
        var tasks = await _reviewService.LoadReviewsAsync(EventItem.EventId);
        tasks.ForEach(ReviewItems.Add);
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

    private async Task SendReviewAsync()
    {
        try
        {
            int eventId = _eventItem.EventId;
            bool success = await _reviewService.SendReviewAsync(eventId, Rating, ReviewText);

            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Review submitted successfully.", "OK");
                ReviewText = string.Empty;
                Rating = 0;
                _ = LoadReviewsAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to submit review.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error submitting review: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while submitting the review.", "OK");
        }
    }
}
