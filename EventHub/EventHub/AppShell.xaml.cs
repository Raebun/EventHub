using EventHub.Views;

namespace EventHub
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();

			Routing.RegisterRoute(nameof(EventDetail), typeof(EventDetail));
			Routing.RegisterRoute(nameof(Order), typeof(Order));
			Routing.RegisterRoute(nameof(Login), typeof(Login));
			Routing.RegisterRoute(nameof(Profile), typeof(Profile));
		}
	}
}
