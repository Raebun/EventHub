using EventHub.Models;
using Shared.Models;

namespace EventHub.Services.Interfaces;

public interface IUserService
{
	Task<UserInfo> GetuserInfoAsync();
	Task<bool> UpdateUserInfoAsync(UserUpdate updatedUser);
    Task<bool> UpdateProfilePictureAsync(Stream imageStream);
}