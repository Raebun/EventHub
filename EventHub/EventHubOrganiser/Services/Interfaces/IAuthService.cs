using Shared.Models;

namespace EventHubOrganiser.Services.Interfaces;

public interface IAuthService
{
    Task<bool> IsAuthenticatedAsync();
    Task<TokenResponse> Login(string email, string password);
}