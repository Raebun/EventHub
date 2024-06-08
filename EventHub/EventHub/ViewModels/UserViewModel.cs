using EventHub.Services;
using EventHub.Services.Interfaces;
using Shared.Models;
using System.Windows.Input;

namespace EventHub.ViewModels;

public class UserViewModel : BaseViewModel
{
    private readonly MessagingService _messagingService;
    private readonly IUserService _userService;
    private string _profileImagePath;
    private string _profilePictureUrl;

    public ICommand SaveChangesCommand { get; }
    public ICommand TakePhotoCommand { get; }
    public ICommand UploadPhotoCommand { get; }
    public ICommand SaveProfilePictureCommand { get; }

    private UserUpdate _updateUser;

    public UserUpdate UpdateUser
	{
		get { return _updateUser; }
		set
		{
			if (_updateUser != value)
			{
				_updateUser = value;
				OnPropertyChanged();
			}
		}
	}

    public string ProfileImagePath
    {
        get { return _profileImagePath; }
        set
        {
            if (_profileImagePath != value)
            {
                _profileImagePath = value;
                OnPropertyChanged();
            }
        }
    }

    public string ProfilePictureUrl
    {
        get { return _profilePictureUrl; }
        set
        {
            if (_profilePictureUrl != value)
            {
                _profilePictureUrl = value;
                OnPropertyChanged();
            }
        }
    }


    public UserViewModel(IUserService userService, MessagingService messagingService)
	{
        _messagingService = messagingService;
        _userService = userService;
        _updateUser = new UserUpdate();
        SaveChangesCommand = new Command(async () => await UpdateUserAsync());
        TakePhotoCommand = new Command(async () => await TakePhotoAsync());
        UploadPhotoCommand = new Command(async () => await UploadPhotoAsync());
        SaveProfilePictureCommand = new Command(async () => await SaveProfilePictureAsync());
        _ = GetUserInfoAsync();
    }

	public async Task GetUserInfoAsync()
	{
		var userInfo = await _userService.GetuserInfoAsync();
		if (userInfo != null)
		{
			UpdateUser.Firstname = userInfo.FirstName;
			UpdateUser.Lastname = userInfo.LastName;
			UpdateUser.Email = userInfo.Email;
            ProfilePictureUrl = userInfo.ProfilePictureUrl;
            OnPropertyChanged(nameof(UpdateUser));
            OnPropertyChanged(nameof(ProfilePictureUrl));
        }
	}

	public async Task UpdateUserAsync()
	{
		bool success = await _userService.UpdateUserInfoAsync(UpdateUser);
		if (success)
		{
			_messagingService.NotifyProfileUpdated();
			await Application.Current.MainPage.DisplayAlert("Success", "User information updated successfully.", "OK");
			UpdateUser.Password = "";
			await Shell.Current.GoToAsync("//Settings");
		}
		else
		{
			await Application.Current.MainPage.DisplayAlert("Failed", "Failed to update user information.", "OK");
		}
	}

    private async Task TakePhotoAsync()
    {
        try
        {
            var result = await MediaPicker.CapturePhotoAsync();
            if (result != null)
            {
                await SavePhotoAsync(result);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async Task UploadPhotoAsync()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick a profile picture"
            });

            if (result != null)
            {
                await SavePhotoAsync(result);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async Task SavePhotoAsync(FileResult result)
    {
        var stream = await result.OpenReadAsync();
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "profile.jpg");
        using (var fileStream = File.Create(filePath))
        {
            await stream.CopyToAsync(fileStream);
        }
        ProfileImagePath = filePath;
    }

    private async Task SaveProfilePictureAsync()
    {
        try
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "profile.jpg");
            if (File.Exists(filePath))
            {
                using var stream = File.OpenRead(filePath);

                var success = await _userService.UpdateProfilePictureAsync(stream);

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Profile picture saved successfully.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to save profile picture.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No profile picture available to save.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }


}