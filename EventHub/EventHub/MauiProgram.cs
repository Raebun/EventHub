using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

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

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
