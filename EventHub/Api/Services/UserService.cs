using Api.Services.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Models;

namespace Api.Services
{
	/// <summary>
	/// Service for user-related operations.
	/// </summary>
	public class UserService : IUserService
	{
		private readonly DataContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


		/// <inheritdoc />
		public async Task<List<User>> GetUsersAsync()
		{
			return await _context.Users.ToListAsync();
		}

		/// <inheritdoc />
		public async Task<User> GetUserByIdAsync(string id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task UpdateUserAsync(string id, UserUpdate updatedUser)
		{
			var existingUser = await _context.Users.FindAsync(id);
			if (existingUser == null)
				throw new ArgumentException("User not found.");

			existingUser.FirstName = updatedUser.Firstname;
			existingUser.LastName = updatedUser.Lastname;
			existingUser.Email = updatedUser.Email;

			// Update password only if it's not empty
			if (!string.IsNullOrEmpty(updatedUser.Password))
			{
				existingUser.PasswordHash = updatedUser.Password;
			}

			await _context.SaveChangesAsync();
		}
	}
}
