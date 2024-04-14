using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class TicketController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
