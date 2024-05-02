using EventHub.Models;
using EventHub.ViewModels;

namespace EventHub.Views;

[QueryProperty(nameof(Events), "Events")]
public partial class Order : ContentPage
{
	public Order(OrderViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}