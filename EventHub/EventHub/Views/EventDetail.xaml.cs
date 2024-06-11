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
        viewModel.AnimateAddToFavoritesButton += async () => await AnimateAddToFavoritesButton();
    }

    private async Task AnimateAddToFavoritesButton()
    {
        await AddToFavoritesButton.ScaleTo(1.1, 100);
        await AddToFavoritesButton.ScaleTo(1, 100);
    }
}