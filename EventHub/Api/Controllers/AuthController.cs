using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Models;

namespace Api.Controllers
{
	public class AuthController : Controller
	{
		private readonly UserManager<User> _userManager;

		public AuthController(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		[HttpPost("registerUser")]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			var user = new User
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				UserName = model.Email,
				PasswordHash = model.Password
			};

			var result = await _userManager.CreateAsync(user, user.PasswordHash!);
			if (result.Succeeded)
				return Ok("Registration made successfully");

			return BadRequest(result);
		}
	}
}
