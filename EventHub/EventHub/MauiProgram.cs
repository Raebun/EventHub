﻿using CommunityToolkit.Maui;
using EventHub.Services;
using EventHub.Services.Interfaces;
using EventHub.ViewModels;
using EventHub.Views;

namespace EventHub
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();

			builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("Inter-Regular.ttf", "InterRegular");
					fonts.AddFont("Inter-Semibold.ttf", "InterSemibold");
				});

			// Services
			builder.Services.AddSingleton<IEventService, EventService>();
			builder.Services.AddSingleton<IOrderService, OrderService>();
			builder.Services.AddSingleton<ISearchService, SearchService>();
			builder.Services.AddSingleton<ITicketService, TicketService>();
			builder.Services.AddSingleton<IUserService, UserService>();
			builder.Services.AddSingleton<IFavoriteService, FavoriteService>();
			builder.Services.AddSingleton<IReviewService, ReviewService>();
			builder.Services.AddSingleton<MessagingService>();

			// Views
			builder.Services.AddSingleton<Home>();
			builder.Services.AddTransient<EventDetail>();
			builder.Services.AddTransient<Order>();
			builder.Services.AddSingleton<Tickets>();
			builder.Services.AddSingleton<Settings>();
			builder.Services.AddSingleton<Profile>();
			builder.Services.AddSingleton<Favorites>();

			// ViewModels
			builder.Services.AddSingleton<HomeViewModel>();
			builder.Services.AddTransient<EventDetailViewModel>();
			builder.Services.AddTransient<OrderViewModel>();
			builder.Services.AddSingleton<TicketViewModel>();
			builder.Services.AddSingleton<SettingsViewModel>();
			builder.Services.AddSingleton<UserViewModel>();
			builder.Services.AddSingleton<FavoritesViewModel>();

			return builder.Build();
		}
	}
}
