using EventHub.Models;

namespace EventHub.Services.Interfaces
{
	public interface IEventService
	{
		Task<List<Events>> LoadEventsAsync();
		Task<UserInfo> UpdateUserInfoAsync();
	}
}
