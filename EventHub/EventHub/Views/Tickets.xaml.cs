using EventHub.ViewModels;

namespace EventHub.Views;

public partial class Tickets : ContentPage
{
	public Tickets(TicketViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}