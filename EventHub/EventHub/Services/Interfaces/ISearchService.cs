using EventHub.Models;

namespace EventHub.Services.Interfaces;

public interface ISearchService
{
    Task<List<Events>> SortEventsBy(string sortBy);
    Task<List<Events>> FilterEventsBy(string filterBy, string searchTerm);
}
