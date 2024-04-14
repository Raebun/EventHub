using Shared.Entities;

namespace Api.Services.Interfaces
{
	/// <summary>
	/// Interface for user service.
	/// </summary>
	public interface IUserService
	{
		/// <summary>
		/// Retrieves all users asynchronously.
		/// </summary>
		/// <returns>The list of users.</returns>
		Task<List<User>> GetUsersAsync();

		/// <summary>
		/// Retrieves a user by ID asynchronously.
		/// </summary>
		/// <param name="id">The ID of the user.</param>
		/// <returns>The user object if found, otherwise null.</returns>
		Task<User> GetUserByIdAsync(string id);
	}
}
