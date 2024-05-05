namespace EventHubOrganiser
{
    public class Constants
    {
        public static readonly HttpClientHandler HttpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };
        public static string LocalhostUrl = "localhost";
        public static string Scheme = "https";
        public static string Port = "7296";
        public static string RestUrl = $"{Scheme}://{LocalhostUrl}:{Port}/";
    }
}
