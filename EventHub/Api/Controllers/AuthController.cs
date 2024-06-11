using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Controllers
{
    [Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		/// <summary>
		/// Registers a new user.
		/// </summary>
		/// <param name="model">The registration model.</param>
		/// <returns>Ok if registration is successful, otherwise BadRequest.</returns>
		[HttpPost("registerUser")]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			var success = await _authService.RegisterUserAsync(model);
			if (success)
				return Ok("Registration made successfully");
			else
				return BadRequest("Registration failed");
		}
	}
}
