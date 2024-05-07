using EventHub.Models;

namespace EventHub.Services.Interfaces;

public interface ISearchService
{
    Task<List<Events>> SortEventsByPriceAsc();
    Task<List<Events>> SortEventsByPriceDesc();
    Task<List<Events>> SortEventsByPopularity();
    Task<List<Events>> SortEventsByDateAsc();
    Task<List<Events>> SortEventsByDateDesc();
}