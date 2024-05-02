using EventHub.Models;
using EventHub.ViewModels;

namespace EventHub.Views;

[QueryProperty(nameof(Events), "Events")]
public partial class EventDetail : ContentPage
{
	public EventDetail(EventDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}