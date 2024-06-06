using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EventHub.Models;
using EventHub.Services.Interfaces;
using Shared.Models;
using SkiaSharp;

namespace EventHub.Services;

public class UserService : IUserService
{
	readonly HttpClient _httpClient;
	readonly JsonSerializerOptions _serializerOptions;

	public UserService()
	{
		_httpClient = new HttpClient(Constants.HttpClientHandler);
		_serializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};
	}

    public async Task<UserInfo> GetuserInfoAsync()
    {
        try
        {
            string authToken = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
                var response = await _httpClient.GetAsync(uri + "User/me");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    UserInfo userInfo = JsonSerializer.Deserialize<UserInfo>(content, _serializerOptions);
                    return userInfo;
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve user information: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving user information: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> UpdateUserInfoAsync(UserUpdate updatedUser)
	{
		try
		{
			string userId = await SecureStorage.GetAsync("user_id");
			string authToken = await SecureStorage.GetAsync("auth_token");
			if (!string.IsNullOrEmpty(authToken))
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

				Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
				var jsonUserInfo = JsonSerializer.Serialize(updatedUser, _serializerOptions);
				var content = new StringContent(jsonUserInfo, Encoding.UTF8, "application/json");

				var response = await _httpClient.PutAsync(uri + $"User/{userId}", content);

				if (response.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					Console.WriteLine($"Failed to update user information: {response.StatusCode}");
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error updating user information: {ex.Message}");
		}
		return false;
	}

    public static byte[] ResizeAndCompressImage(Stream inputImageStream, int width, int height, int quality)
    {
        using var original = SKBitmap.Decode(inputImageStream);
        using var resized = original.Resize(new SKImageInfo(width, height), SKFilterQuality.High);

        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, quality);

        return data.ToArray();
    }

    public async Task<bool> UpdateProfilePictureAsync(Stream imageStream)
    {
        try
        {
            string userId = await SecureStorage.GetAsync("user_id");
            string authToken = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

                byte[] resizedImage = ResizeAndCompressImage(imageStream, 400, 400, 80);
                string base64Image = Convert.ToBase64String(resizedImage);

                var jsonPayload = JsonSerializer.Serialize(new { profilePictureUrl = base64Image }, _serializerOptions);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{uri}User/{userId}/profile-picture", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to update profile picture: {response.StatusCode}, Details: {errorContent}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating profile picture: {ex.Message}");
        }
        return false;
    }

}
