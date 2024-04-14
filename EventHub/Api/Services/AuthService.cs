using Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using Shared.Models;

namespace Api.Services
{
	/// <summary>
	/// Service for authentication-related operations.
	/// </summary>
	public class AuthService : IAuthService
	{
		private readonly UserManager<User> _userManager;

		public AuthService(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		/// <inheritdoc />
		public async Task<bool> RegisterUserAsync(RegisterModel model)
		{
			var user = new User
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				UserName = model.Email,
				PasswordHash = model.Password
			};

			var result = await _userManager.CreateAsync(user, user.PasswordHash);
			return result.Succeeded;
		}
	}
}
