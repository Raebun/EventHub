namespace EventHub
{
	public class Constants
	{
		public static readonly HttpClientHandler HttpClientHandler = new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
		};
		public static string LocalhostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
		public static string Scheme = "https";
		public static string Port = "7296";
		public static string RestUrl = $"{Scheme}://{LocalhostUrl}:{Port}/";
	}
}
