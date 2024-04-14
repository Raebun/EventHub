using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class SearchController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
