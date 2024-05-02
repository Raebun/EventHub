using EventHub.Models;
using EventHub.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Transactions;
using System.Windows.Input;

namespace EventHub.ViewModels;

[QueryProperty(nameof(EventItem), "Events")]
public class OrderViewModel : BaseViewModel
{
	private Events _eventItem;
	private int _numberOfTickets;
	public ObservableCollection<TicketHolder> TicketHolders { get; set; }
	public ICommand ConfirmCommand { get; private set; }
	private readonly IOrderService _orderService;

	public OrderViewModel(IOrderService service)
	{
		_orderService = service;
		TicketHolders = new ObservableCollection<TicketHolder>();
		ConfirmCommand = new Command(ConfirmAndBuyTickets);
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

	public int NumberOfTickets
	{
		get => _numberOfTickets;
		set
		{
			_numberOfTickets = value;
			GenerateTicketHolders();
			OnPropertyChanged();
		}
	}

	private void GenerateTicketHolders()
	{
		TicketHolders.Clear();
		for (int i = 0; i < NumberOfTickets; i++)
		{
			TicketHolders.Add(new TicketHolder());
		}
	}

	private async Task<bool> BuyTicketAndHandleResponse(int eventId, string userId, string firstName, string lastName)
	{
		try
		{
			bool response = await _orderService.BuyTicket(eventId, userId, firstName, lastName);

			if (!response)
			{
				await Application.Current.MainPage.DisplayAlert("Error", $"Failed to purchase ticket for {firstName} {lastName}", "OK");
				return false;
			}

			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error buying ticket: {ex.Message}");
			return false;
		}
	}

	private async void ConfirmAndBuyTickets()
	{
		try
		{
			string userId = await SecureStorage.GetAsync("user_id");
			string authToken = await SecureStorage.GetAsync("auth_token");

			// Begin transaction
			using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				bool allPurchasesSuccessful = true;

				foreach (var ticketHolder in TicketHolders)
				{
					// Call for each ticket holder
					bool purchaseSuccessful = await BuyTicketAndHandleResponse(EventItem.EventId, userId, ticketHolder.FirstName, ticketHolder.LastName);

					if (!purchaseSuccessful)
					{
						allPurchasesSuccessful = false;
						break; // Exit the loop if any purchase fails
					}
				}

				if (allPurchasesSuccessful)
				{
					transaction.Complete();

					await Application.Current.MainPage.DisplayAlert("Success", "Purchase successful", "OK");
					await Shell.Current.GoToAsync("//Home");
				}
				else
				{
					transaction.Dispose();

					await Application.Current.MainPage.DisplayAlert("Error", "Failed to purchase tickets", "OK");
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error buying tickets: {ex.Message}");
		}
	}


}
