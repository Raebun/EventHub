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
    private readonly ISearchService _searchService;
    private readonly MessagingService _messagingService;
	public ObservableCollection<Events> EventItems { get; set; } = [];
	private string? _fullName;
	public ICommand SelectEventCommand { get; set; }
    public ICommand SortByPriceAscCommand { get; }
    public ICommand SortByPriceDescCommand { get; }
    public ICommand SortByPopularityCommand { get; }
    public ICommand SortByDateAscCommand { get; }
    public ICommand SortByDateDescCommand { get; }
    public ICommand FilterByDateCommand { get; }
    public ICommand FilterByNameCommand { get; }
    public ICommand FilterByPriceCommand { get; }
    public ICommand FilterByLocationCommand { get; }

    private int _selectedIndex;
    public int SelectedIndex
    {
        get { return _selectedIndex; }
        set
        {
            if (_selectedIndex != value)
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                SortBySelectedIndex();
            }
        }
    }


    public string FullName
	{
		get { return _fullName; }
		set { SetProperty(ref _fullName, value); }
	}

	public HomeViewModel(
        ISearchService searchService,
        IEventService service, 
        MessagingService messagingService
     ) {
        _searchService = searchService;
        _messagingService = messagingService;
		_messagingService.ProfileUpdated += (sender, args) =>
		{
			UpdateUserInfoAsync();
		};
		_eventService = service;
		SelectEventCommand = new Command<Events>(async (eventItem) => await SelectionChanged(eventItem));
        SortByPriceAscCommand = new Command(async () => await SortByPriceAsc());
        SortByPriceDescCommand = new Command(async () => await SortByPriceDesc());
        SortByPopularityCommand = new Command(async () => await SortByPopularity());
        SortByDateAscCommand = new Command(async () => await SortByDateAsc());
        SortByDateDescCommand = new Command(async () => await SortByDateDesc());
        FilterByDateCommand = new Command<string>(async (date) => await FilterByDate(date));
        FilterByNameCommand = new Command<string>(async (name) => await FilterByName(name));
        FilterByPriceCommand = new Command<float>(async (price) => await FilterByPrice(price));
        FilterByLocationCommand = new Command<string>(async (location) => await FilterByLocation(location));
        _ = LoadEventsAsync();
		_ = UpdateUserInfoAsync();
	}

    private async Task SortBySelectedIndex()
    {
        switch (SelectedIndex)
        {
            case 0:
                await SortByPriceAsc();
                break;
            case 1:
                await SortByPriceDesc();
                break;
            case 2:
                await SortByPopularity();
                break;
            case 3:
                await SortByDateAsc();
                break;
            case 4:
                await SortByDateDesc();
                break;
            default:
                break;
        }
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

    private async Task SortByPriceAsc()
    {
        var sortedEvents = await _searchService.SortEventsByPriceAsc();
        EventItems.Clear();
        sortedEvents.ForEach(EventItems.Add);
    }
    private async Task SortByPriceDesc()
    {
        var sortedEvents = await _searchService.SortEventsByPriceDesc();
        EventItems.Clear();
        sortedEvents.ForEach(EventItems.Add);
    }

    private async Task SortByPopularity()
    {
        var sortedEvents = await _searchService.SortEventsByPopularity();
        EventItems.Clear();
        sortedEvents.ForEach(EventItems.Add);
    }

    private async Task SortByDateAsc()
    {
        var sortedEvents = await _searchService.SortEventsByDateAsc();
        EventItems.Clear();
        sortedEvents.ForEach(EventItems.Add);
    }

    private async Task SortByDateDesc()
    {
        var sortedEvents = await _searchService.SortEventsByDateDesc();
        EventItems.Clear();
        sortedEvents.ForEach(EventItems.Add);
    }

    private async Task FilterByDate(string date)
    {
        // Implement
    }

    private async Task FilterByName(string name)
    {
        // Implement
    }

    private async Task FilterByPrice(float price)
    {
        // Implement
    }

    private async Task FilterByLocation(string location)
    {
        // Implement
    }

}