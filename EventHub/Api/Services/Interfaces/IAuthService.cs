using Shared.Models;

namespace Api.Services.Interfaces
{
	/// <summary>
	/// Interface for authentication service.
	/// </summary>
	public interface IAuthService
	{
		/// <summary>
		/// Registers a new user.
		/// </summary>
		/// <param name="model">The registration model.</param>
		/// <returns>True if registration is successful, otherwise false.</returns>
		Task<bool> RegisterUserAsync(RegisterModel model);
	}
}
