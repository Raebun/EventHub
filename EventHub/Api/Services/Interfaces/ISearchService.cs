using Shared.Entities;

namespace Api.Services.Interfaces;

public interface ISearchService
{
    Task<List<Event>> SearchEventsByNameAsync(string name);
    Task<List<Event>> SearchEventsByDateAsync(DateTime date);
    Task<List<Event>> SearchEventsByLocationAsync(string location);
    Task<List<Event>> SearchEventsByPriceAsync(float price, float tolerance = 0.1f);
    Task<List<Event>> SortEventsByPriceAsc();
    Task<List<Event>> SortEventsByPriceDesc();
    Task<List<Event>> SortEventsByPopularity();
    Task<List<Event>> SortEventsByDateAsc();
    Task<List<Event>> SortEventsByDateDesc();
}