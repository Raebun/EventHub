using EventHub.Models;
using EventHub.Views;
using System.Windows.Input;

namespace EventHub.ViewModels;

[QueryProperty(nameof(EventItem), "Events")]
public class EventDetailViewModel : BaseViewModel
{
	private Events _eventItem;
	public ICommand BookNowCommand { get; private set; }

	public EventDetailViewModel() 
	{
		BookNowCommand = new Command(async () => await SelectionChanged(_eventItem));
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
}
