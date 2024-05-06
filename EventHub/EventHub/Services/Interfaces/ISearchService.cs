using EventHub.Models;

namespace EventHub.Services.Interfaces;

public interface ISearchService
{
    Task<List<Events>> SortEventsByPriceAsc();
    Task<List<Events>> SortEventsByPriceDesc();
}