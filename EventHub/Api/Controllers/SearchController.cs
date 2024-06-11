using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<Event>>> SearchEvents([FromQuery] string type, [FromQuery] string query)
        {
            List<Event> events = new List<Event>();
            switch (type.ToLower())
            {
                case "date":
                    if (DateTime.TryParse(query, out DateTime date))
                    {
                        events = await _searchService.SearchEventsByDateAsync(date);
                    }
                    else
                    {
                        return BadRequest("Invalid date format. Date format should be YYYY-MM-DD.");
                    }
                    break;
                case "name":
                    events = await _searchService.SearchEventsByNameAsync(query);
                    break;
                case "price":
                    if (float.TryParse(query, out float price))
                    {
                        events = await _searchService.SearchEventsByPriceAsync(price);
                    }
                    else
                    {
                        return BadRequest("Invalid price format. Price should be a valid number.");
                    }
                    break;
                case "location":
                    events = await _searchService.SearchEventsByLocationAsync(query);
                    break;
                default:
                    return BadRequest("Invalid search type. Supported types: Date, Name, Price, Location.");
            }

            return Ok(events);
        }

        [HttpGet("sort")]
        public async Task<ActionResult<List<Event>>> SortEvents([FromQuery] string sortBy)
        {
            List<Event> sortedEvents = new List<Event>();

            switch (sortBy.ToLower())
            {
                case "priceasc":
                    sortedEvents = await _searchService.SortEventsByPriceAsc();
                    break;
                case "pricedesc":
                    sortedEvents = await _searchService.SortEventsByPriceDesc();
                    break;
                case "popular":
                    sortedEvents = await _searchService.SortEventsByPopularity();
                    break;
                case "dateasc":
                    sortedEvents = await _searchService.SortEventsByDateAsc();
                    break;
                case "datedesc":
                    sortedEvents = await _searchService.SortEventsByDateDesc();
                    break;
                default:
                    return BadRequest("Invalid sorting criteria.");
            }

            return Ok(sortedEvents);
        }
    }
}
