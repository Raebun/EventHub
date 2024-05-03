using EventHub.ViewModels;

namespace EventHub.Views;

public partial class Favorites : ContentPage
{
	public Favorites(FavoritesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}