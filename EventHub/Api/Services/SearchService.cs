using Api.Services.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Api.Services;

public class SearchService : ISearchService
{
    private readonly DataContext _context;

    public SearchService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> SearchEventsByNameAsync(string name)
    {
        return await _context.Events.Where(e => e.EventName.Contains(name)).ToListAsync();
    }

    public async Task<List<Event>> SearchEventsByDateAsync(DateTime date)
    {
        return await _context.Events.Where(e => e.EventDate.Date == date.Date).ToListAsync();
    }

    public async Task<List<Event>> SearchEventsByLocationAsync(string location)
    {
        return await _context.Events.Where(e => e.Location.Contains(location)).ToListAsync();
    }

    public async Task<List<Event>> SearchEventsByPriceAsync(float price, float tolerance = 0.1f)
    {
        float minPrice = price - tolerance;
        float maxPrice = price + tolerance;

        return await _context.Events.Where(e => e.TicketPrice >= minPrice && e.TicketPrice <= maxPrice).ToListAsync();
    }

    public async Task<List<Event>> SortEventsByPriceAsc()
    {
        return await _context.Events.OrderBy(e => e.TicketPrice).ToListAsync();
    }

    public async Task<List<Event>> SortEventsByPriceDesc()
    {
        return await _context.Events.OrderByDescending(e => e.TicketPrice).ToListAsync();
    }

    public async Task<List<Event>> SortEventsByPopularity()
    {
        var eventTicketCounts = await GetTicketCountsForAllEventsAsync();
        var eventsWithTicketsSoldIds = eventTicketCounts.Keys.ToList();

        var sortedEvents = eventTicketCounts
            .OrderByDescending(kv => kv.Value)
            .Select(kv => kv.Key)
            .ToList();

        var events = await _context.Events.Where(e => sortedEvents.Contains(e.EventId)).ToListAsync();

        var eventsWithZeroTickets = await _context.Events
            .Where(e => !eventsWithTicketsSoldIds.Contains(e.EventId))
            .ToListAsync();

        events.AddRange(eventsWithZeroTickets);

        return events;
    }

    private async Task<Dictionary<int, int>> GetTicketCountsForAllEventsAsync()
    {
        var ticketCounts = await _context.Tickets
            .GroupBy(t => t.EventId)
            .Select(g => new { EventId = g.Key, TicketCount = g.Count() })
            .ToDictionaryAsync(x => x.EventId, x => x.TicketCount);

        return ticketCounts;
    }


    public async Task<List<Event>> SortEventsByDateAsc()
    {
        return await _context.Events.OrderBy(e => e.EventDate).ToListAsync();
    }

    public async Task<List<Event>> SortEventsByDateDesc()
    {
        return await _context.Events.OrderByDescending(e => e.EventDate).ToListAsync();
    }
}