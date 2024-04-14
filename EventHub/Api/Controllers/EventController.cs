using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class EventController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
