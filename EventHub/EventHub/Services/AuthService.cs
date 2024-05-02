namespace EventHub.Services
{
	public class AuthService
	{
		public async Task<bool> IsAuthenticatedAsync()
		{
			await Task.Delay(2000);

			var authToken = await SecureStorage.GetAsync("auth_token");
			if (!string.IsNullOrEmpty(authToken))
			{
				return true;
			}

			return false;
		}

	}
}
