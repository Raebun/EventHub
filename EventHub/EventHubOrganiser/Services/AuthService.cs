using EventHubOrganiser.Models;
using EventHubOrganiser.Services.Interfaces;
using Microsoft.JSInterop;
using Shared.Models;
using System.Text;
using System.Text.Json;

namespace EventHubOrganiser.Services
{
    public class AuthService : IAuthService
    {
        readonly HttpClient _httpClient;
        readonly JsonSerializerOptions _serializerOptions;
        private readonly IJSRuntime _js;

        public AuthService(IJSRuntime js)
        {
            _js = js;
            _httpClient = new HttpClient(Constants.HttpClientHandler);
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authToken = await _js.InvokeAsync<string>("localStorage.getItem", "auth_token");
            return !string.IsNullOrEmpty(authToken);
        }

        public async Task<TokenResponse?> Login(string email, string password)
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

            var loginModel = new LoginModel { Email = email, Password = password };
            var json = JsonSerializer.Serialize(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri + "login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<TokenResponse>(responseStream, _serializerOptions);
			}

            return null;
        }
    }
}
