using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class FavoriteController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
