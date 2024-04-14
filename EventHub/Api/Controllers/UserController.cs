using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : Controller
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		/// <summary>
		/// Retrieves all users.
		/// </summary>
		/// <returns>The list of users.</returns>
		[HttpGet]
		public async Task<ActionResult<List<User>>> GetUsers()
		{
			var users = await _userService.GetUsersAsync();
			return Ok(users);
		}

		/// <summary>
		/// Retrieves a user by ID.
		/// </summary>
		/// <param name="id">The ID of the user.</param>
		/// <returns>The user object if found, otherwise returns NotFoundResult.</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(string id)
		{
			var user = await _userService.GetUserByIdAsync(id);
			if (user == null)
				return NotFound("User not found");

			return Ok(user);
		}
	}
}
