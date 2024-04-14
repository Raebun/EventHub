using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : Controller
	{
		private readonly DataContext _context;

		public UserController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<List<User>>> GetUsers()
		{
			var users = await _context.Users.ToListAsync();
			return Ok(users);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(string id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
				return NotFound("User not found");

			return Ok(user);
		}
	}
}
