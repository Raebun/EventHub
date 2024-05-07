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

    private int _selectedFilterIndex;
    public int SelectedFilterIndex
    {
        get { return _selectedFilterIndex; }
        set
        {
            if (_selectedFilterIndex != value)
            {
                _selectedFilterIndex = value;
                OnPropertyChanged(nameof(SelectedFilterIndex));
                ApplyFilter();
            }
        }
    }

    private string _searchTerm;
    public string SearchTerm
    {
        get { return _searchTerm; }
        set
        {
            if (_searchTerm != value)
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
                ApplyFilter();
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
        _ = LoadEventsAsync();
		_ = UpdateUserInfoAsync();
	}

    private async Task ApplyFilter()
    {
        string filter = GetFilterByIndex(SelectedFilterIndex);
        await FilterBy(filter, SearchTerm);
    }

    private string GetFilterByIndex(int index)
    {
        switch (index)
        {
            case 0: return "name";
            case 1: return "price";
            case 2: return "location";
            case 3: return "date";
            default: return string.Empty;
        }
    }

    private async Task SortBySelectedIndex()
    {
        switch (SelectedIndex)
        {
            case 0:
                await SortBy("priceasc");
                break;
            case 1:
                await SortBy("pricedesc");
                break;
            case 2:
                await SortBy("popular");
                break;
            case 3:
                await SortBy("dateasc");
                break;
            case 4:
                await SortBy("datedesc");
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

    private async Task SortBy(string sortBy)
    {
        var sortedEvents = await _searchService.SortEventsBy(sortBy);
        EventItems.Clear();
        sortedEvents.ForEach(EventItems.Add);
    }

    private async Task FilterBy(string filterBy, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            await LoadEventsAsync();
            return;
        }

        var filteredEvents = await _searchService.FilterEventsBy(filterBy, searchTerm);
        EventItems.Clear();
        filteredEvents.ForEach(EventItems.Add);
    }
}