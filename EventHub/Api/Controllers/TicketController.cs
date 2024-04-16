using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Models;

namespace Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class TicketController : Controller
	{
		private readonly ITicketService _ticketService;

		public TicketController(ITicketService ticketService)
		{
			_ticketService = ticketService;
		}

		[HttpGet("event/{eventId}")]
		public async Task<ActionResult<List<Ticket>>> GetTicketsForEvent(int eventId)
		{
			var tickets = await _ticketService.GetTicketsForEventAsync(eventId);
			return Ok(tickets);
		}

		[HttpGet("user/{userId}")]
		public async Task<ActionResult<List<Ticket>>> GetTicketsForUser(Guid userId)
		{
			var tickets = await _ticketService.GetTicketsForUserAsync(userId);
			return Ok(tickets);
		}

		[HttpPost("purchase/{eventId}/{userId}")]
		public async Task<ActionResult<Ticket>> PurchaseTicket(int eventId, Guid userId, [FromBody] TicketCreateModel ticketModel)
		{
			try
			{
				var purchasedTicket = await _ticketService.PurchaseTicketAsync(ticketModel, eventId, userId);
				return Ok(purchasedTicket);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while processing the request.");
			}
		}
	}
}
