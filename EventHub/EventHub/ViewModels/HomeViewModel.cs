using CommunityToolkit.Mvvm.ComponentModel;
using EventHub.Models;
using EventHub.Services;
using EventHub.Services.Interfaces;
using EventHub.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EventHub.ViewModels;

public class HomeViewModel : ObservableObject
{
	private readonly IEventService _eventService;
	private readonly MessagingService _messagingService;
	public ObservableCollection<Events> EventItems { get; set; } = [];
	private string? _fullName;
	public ICommand SelectEventCommand { get; set; }


	public string FullName
	{
		get { return _fullName; }
		set { SetProperty(ref _fullName, value); }
	}

	public HomeViewModel(IEventService service, MessagingService messagingService)
	{
		_messagingService = messagingService;
		_messagingService.ProfileUpdated += (sender, args) =>
		{
			UpdateUserInfoAsync();
		};
		_eventService = service;
		SelectEventCommand = new Command<Events>(async (eventItem) => await SelectionChanged(eventItem));
		_ = LoadEventsAsync();
		_ = UpdateUserInfoAsync();
	}

	public async Task UpdateUserInfoAsync()
	{
		var userInfo = await _eventService.UpdateUserInfoAsync();
		if (userInfo != null)
		{
			FullName = $"{userInfo.FirstName} {userInfo.LastName}";
		}
	}

	public async Task LoadEventsAsync()
	{
		EventItems.Clear();
		var tasks = await _eventService.LoadEventsAsync();
		tasks.ForEach(EventItems.Add);
	}

	private async Task SelectionChanged(Events eventItem)
	{
		if (eventItem == null) return;

		var navigationParameter = new Dictionary<string, object>
			{
				{ nameof(Events), eventItem }
			};
		await Shell.Current.GoToAsync(nameof(EventDetail), navigationParameter);
	}
}