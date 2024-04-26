using Api.Services.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using System.Security.Claims;

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
	}
}
