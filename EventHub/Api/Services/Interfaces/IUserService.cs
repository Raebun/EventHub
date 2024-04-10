namespace Api.Services.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(int id);
		Task<IEnumerable<User>> AddUserAsync(User user);
		Task<IEnumerable<User>> UpdateUserAsync(User user);
		Task<IEnumerable<User>> DeleteUserAsync(int id);
	}
}
