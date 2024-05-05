using EventHubOrganiser.Models;
using EventHubOrganiser.Services.Interfaces;
using Microsoft.JSInterop;
using Shared.Models;
using System.Net.Http.Headers;
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
                var responseToken = await JsonSerializer.DeserializeAsync<TokenResponse>(responseStream, _serializerOptions);

                // Note to self: @rendermode InteractiveServer  is needed to avoid interop JS error
                await _js.InvokeVoidAsync("localStorage.setItem", "auth_token", responseToken.accessToken);
                var authToken = await _js.InvokeAsync<string>("localStorage.getItem", "auth_token");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                var userInfoResponse = await _httpClient.GetAsync(uri + "User/me");
                if (userInfoResponse.IsSuccessStatusCode)
                {
                    var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                    var userInfo = JsonSerializer.Deserialize<UserId>(userInfoContent);
                    await _js.InvokeVoidAsync("localStorage.setItem", "user_id", userInfo.id);
                }

                return responseToken;
            }

            return null;
        }
    }
}
