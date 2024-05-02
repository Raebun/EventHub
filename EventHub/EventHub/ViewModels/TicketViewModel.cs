using CommunityToolkit.Mvvm.ComponentModel;
using EventHub.Models;
using EventHub.Services.Interfaces;
using QRCoder;
using System.Collections.ObjectModel;
using static QRCoder.QRCodeGenerator;

namespace EventHub.ViewModels;

public class TicketViewModel : ObservableObject
{
	private readonly ITicketService _ticketService;
	public ObservableCollection<Ticket> TicketItems { get; set; } = [];

	public TicketViewModel(ITicketService service)
	{
		_ticketService = service;
		_ = LoadEventsAsync();
	}

	public async Task LoadEventsAsync()
	{
		TicketItems.Clear();
		string userId = await SecureStorage.GetAsync("user_id");
		var tasks = await _ticketService.LoadTicketsAsync(userId);
		tasks.ForEach(TicketItems.Add);
	}
}
