using NSubstitute.ExceptionExtensions;
using System.Net;

namespace UnitTests;

public class EventsViewModelTest
{
    [Fact]
    public async Task LoadEventsAsync_ShouldLoadEvents()
    {
        // Arrange
        var eventService = Substitute.For<IEventService>();
        var searchService = Substitute.For<ISearchService>();
        var messagingService = Substitute.For<MessagingService>();
        var viewModel = new HomeViewModel(searchService, eventService, messagingService);

        var events = new List<EventHub.Models.Events>
            {
                new EventHub.Models.Events { EventId = 1, EventName = "Event 1" },
                new EventHub.Models.Events { EventId = 2, EventName = "Event 2" }
            };

        eventService.LoadEventsAsync().Returns(events);

        // Act
        await viewModel.LoadEventsAsync();

        // Assert
        viewModel.EventItems.Should().HaveCount(2);
        viewModel.EventItems.Should().ContainEquivalentOf(new EventHub.Models.Events { EventId = 1, EventName = "Event 1" });
        viewModel.EventItems.Should().ContainEquivalentOf(new EventHub.Models.Events { EventId = 2, EventName = "Event 2" });
    }

    [Fact]
    public async Task LoadEventsAsync_ShouldHandleFailure()
    {
        // Arrange
        var eventService = Substitute.For<IEventService>();
        var searchService = Substitute.For<ISearchService>();
        var messagingService = Substitute.For<MessagingService>();
        var viewModel = new HomeViewModel(searchService, eventService, messagingService);

        eventService.LoadEventsAsync().Throws(new Exception("Failed to load events"));

        // Act
        Func<Task> action = viewModel.LoadEventsAsync;

        // Assert
        await action.Should().ThrowAsync<Exception>().WithMessage("Failed to load events");
        viewModel.EventItems.Should().BeEmpty();
    }

    [Fact]
    public async Task ApplyFilter_ShouldFilterEventItemsBasedOnSelectedFilterIndexAndSearchTerm()
    {
        // Arrange
        var eventService = Substitute.For<IEventService>();
        var searchService = Substitute.For<ISearchService>();
        var messagingService = Substitute.For<MessagingService>();
        var viewModel = new HomeViewModel(searchService, eventService, messagingService);
        var events = new List<EventHub.Models.Events>
        {
            new EventHub.Models.Events { EventId = 1, EventName = "Event 1", Location = "Location A" },
            new EventHub.Models.Events { EventId = 2, EventName = "Event 2", Location = "Location B" }
        };
        eventService.LoadEventsAsync().Returns(events);
        searchService.FilterEventsBy(Arg.Any<string>(), Arg.Any<string>()).Returns(
            callInfo =>
            {
                var filterBy = callInfo.ArgAt<string>(0);
                var searchTerm = callInfo.ArgAt<string>(1);
                return events.Where(e => e.Location.Contains(searchTerm)).ToList();
            });

        // Act
        await viewModel.LoadEventsAsync();
        viewModel.SelectedFilterIndex = 2;
        viewModel.SearchTerm = "B";
        await viewModel.ApplyFilter();

        // Assert
        viewModel.EventItems.Should().ContainSingle();
        viewModel.EventItems[0].Location.Should().Be("Location B");
    }

    [Fact]
    public async Task ApplyFilter_ShouldHandleFailure()
    {
        // Arrange
        var eventService = Substitute.For<IEventService>();
        var searchService = Substitute.For<ISearchService>();
        var messagingService = Substitute.For<MessagingService>();
        var viewModel = new HomeViewModel(searchService, eventService, messagingService);

        var events = new List<EventHub.Models.Events>
        {
            new EventHub.Models.Events { EventId = 1, EventName = "Event 1", Location = "Location A" },
            new EventHub.Models.Events { EventId = 2, EventName = "Event 2", Location = "Location B" }
        };
        eventService.LoadEventsAsync().Returns(events);
        searchService.FilterEventsBy(Arg.Any<string>(), Arg.Any<string>()).Throws(new Exception("Failed to filter events"));

        // Act
        await viewModel.LoadEventsAsync();
        viewModel.SelectedFilterIndex = 2;
        viewModel.SearchTerm = "B";
        Func<Task> action = viewModel.ApplyFilter;

        // Assert
        await action.Should().ThrowAsync<Exception>().WithMessage("Failed to filter events");
        viewModel.EventItems.Should().HaveCount(2);
    }

    [Fact]
    public async Task SortBySelectedIndex_ShouldSortEventsByPriceAscending()
    {
        // Arrange
        var eventService = Substitute.For<IEventService>();
        var searchService = Substitute.For<ISearchService>();
        var messagingService = Substitute.For<MessagingService>();
        var viewModel = new HomeViewModel(searchService, eventService, messagingService);
        var events = new List<EventHub.Models.Events>
        {
            new EventHub.Models.Events { EventId = 1, EventName = "Event 1", TicketPrice = 20.0f },
            new EventHub.Models.Events { EventId = 2, EventName = "Event 2", TicketPrice = 10.0f }
        };
        eventService.LoadEventsAsync().Returns(events);
        searchService.SortEventsBy(Arg.Any<string>()).Returns(
            callInfo =>
            {
                var sortBy = callInfo.ArgAt<string>(0);
                return sortBy == "priceasc" ? events.OrderBy(e => e.TicketPrice).ToList() : events.OrderByDescending(e => e.TicketPrice).ToList();
            });

        // Act
        await viewModel.LoadEventsAsync();
        viewModel.SelectedIndex = 0;
        await viewModel.SortBySelectedIndex();

        // Assert
        viewModel.EventItems.Should().HaveCount(2);
        viewModel.EventItems[0].TicketPrice.Should().Be(10.0f);
        viewModel.EventItems[1].TicketPrice.Should().Be(20.0f);
    }
}
