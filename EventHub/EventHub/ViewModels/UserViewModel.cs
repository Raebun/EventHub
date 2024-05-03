using EventHub.Services.Interfaces;
using Shared.Models;
using System.Windows.Input;

namespace EventHub.ViewModels;

public class UserViewModel : BaseViewModel
{
	public ICommand SaveChangesCommand { get; }
	private UserUpdate _updateUser;
	private readonly IUserService _userService;

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

	public UserViewModel(IUserService userService)
	{
		_userService = userService;
		_updateUser = new UserUpdate();
		SaveChangesCommand = new Command(async () => await UpdateUserAsync());
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
			OnPropertyChanged(nameof(UpdateUser));
		}
	}

	public async Task UpdateUserAsync()
	{
		bool success = await _userService.UpdateUserInfoAsync(UpdateUser);
		if (success)
		{
			await Application.Current.MainPage.DisplayAlert("Success", "User information updated successfully.", "OK");
			UpdateUser.Password = "";
			await Shell.Current.GoToAsync("//Settings");
		}
		else
		{
			Console.WriteLine("UserViewModel Failed");
			await Application.Current.MainPage.DisplayAlert("Failed", "Failed to update user information.", "OK");
		}
	}
}