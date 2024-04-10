using Api.Helpers;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	/// <summary>
	/// Controller for managing user-related operations.
	/// </summary>
	[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		/// <summary>
		/// Retrieves all users from the database.
		/// </summary>
		/// <returns>A collection of all users.</returns>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			var users = await _userService.GetAllUsersAsync();
			return Ok(users);
		}

		/// <summary>
		/// Retrieves a user by their ID from the database.
		/// </summary>
		/// <param name="id">The ID of the user to retrieve.</param>
		/// <returns>The user object if found, null otherwise.</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(int id)
		{
			var user = await _userService.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound("User was not found");
			}
			return Ok(user);
		}

		/// <summary>
		/// Adds a new user to the database.
		/// </summary>
		/// <param name="user">The user object to add.</param>
		/// <returns>A collection of all users after adding the new user.</returns>
		[HttpPost]
		public async Task<ActionResult<IEnumerable<User>>> AddUser(User user)
		{
			string hashedPassword = PasswordHasher.HashPassword(user.PasswordHash);
			user.PasswordHash = hashedPassword;

			var users = await _userService.AddUserAsync(user);
			return Ok(users);
		}

		/// <summary>
		/// Updates an existing user in the database.
		/// </summary>
		/// <param name="user">The updated user object.</param>
		/// <returns>A collection of all users after updating the user.</returns>
		[HttpPut]
		public async Task<ActionResult<IEnumerable<User>>> UpdateUser(User user)
		{
			var users = await _userService.UpdateUserAsync(user);
			return Ok(users);
		}

		/// <summary>
		/// Deletes a user from the database by their ID.
		/// </summary>
		/// <param name="id">The ID of the user to delete.</param>
		/// <returns>A collection of all users after deleting the user.</returns>
		[HttpDelete("{id}")]
		public async Task<ActionResult<IEnumerable<User>>> DeleteUser(int id)
		{
			var users = await _userService.DeleteUserAsync(id);
			return Ok(users);
		}
	}
}
