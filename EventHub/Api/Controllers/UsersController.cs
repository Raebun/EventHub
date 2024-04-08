using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly MyDbContext _context;

		public UsersController(MyDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Retrieves a list of users.
		/// </summary>
		/// <returns>A list of users.</returns>
		[HttpGet(Name = "GetUsers")]
		public ActionResult<IEnumerable<User>> GetUsers()
		{
			return _context.Users.ToList();
		}
	}
}
