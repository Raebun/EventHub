using Api.Services.Interfaces;

namespace Api.Services
{
	public class UserService : IUserService
	{
		private readonly string _connectionString;

		public UserService(string connectionString)
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Retrieves all users from the database.
		/// </summary>
		/// <returns>A collection of all users.</returns>
		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			using var connection = new SqlConnection(_connectionString);
			return await connection.QueryAsync<User>("SELECT * FROM Users");
		}

		/// <summary>
		/// Retrieves a user by their ID from the database.
		/// </summary>
		/// <param name="id">The ID of the user to retrieve.</param>
		/// <returns>The user object if found, null otherwise.</returns>
		public async Task<User> GetUserByIdAsync(int id)
		{
			using var connection = new SqlConnection(_connectionString);
			return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE UserId = @UserId", new { UserId = id });
		}

		/// <summary>
		/// Adds a new user to the database.
		/// </summary>
		/// <param name="user">The user object to add.</param>
		/// <returns>A collection of all users after adding the new user.</returns>
		public async Task<IEnumerable<User>> AddUserAsync(User user)
		{
			using var connection = new SqlConnection(_connectionString);
			await connection.ExecuteAsync("INSERT INTO Users (FirstName, LastName, Email, PasswordHash) VALUES (@FirstName, @LastName, @Email, @PasswordHash)", user);
			return await GetAllUsersAsync();
		}

		/// <summary>
		/// Updates an existing user in the database.
		/// </summary>
		/// <param name="user">The updated user object.</param>
		/// <returns>A collection of all users after updating the user.</returns>
		public async Task<IEnumerable<User>> UpdateUserAsync(User user)
		{
			using var connection = new SqlConnection(_connectionString);
			await connection.ExecuteAsync("UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PasswordHash = @PasswordHash, ProfilePicture = @ProfilePicture WHERE UserId = @UserId", user);
			return await GetAllUsersAsync();
		}

		/// <summary>
		/// Deletes a user from the database by their ID.
		/// </summary>
		/// <param name="id">The ID of the user to delete.</param>
		/// <returns>A collection of all users after deleting the user.</returns>
		public async Task<IEnumerable<User>> DeleteUserAsync(int id)
		{
			using var connection = new SqlConnection(_connectionString);
			await connection.ExecuteAsync("DELETE FROM Users WHERE UserId = @UserId", new { UserId = id });
			return await GetAllUsersAsync();
		}
	}
}
