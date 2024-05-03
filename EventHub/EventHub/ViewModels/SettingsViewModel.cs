﻿using CommunityToolkit.Mvvm.ComponentModel;
using EventHub.Services.Interfaces;
using EventHub.Views;
using System.Windows.Input;

namespace EventHub.ViewModels;

public class SettingsViewModel : ObservableObject
{
	public ICommand LogoutCommand { get; }
	public ICommand NavigateToProfileCommand { get; }

	private readonly IUserService _userService;
	private string _email;
	private string? _fullName;

	public string Email
	{
		get { return _email; }
		set { SetProperty(ref _email, value); }
	}

	public string FullName
	{
		get { return _fullName; }
		set { SetProperty(ref _fullName, value); }
	}

	public SettingsViewModel(IUserService userService)
	{
		_userService = userService;
		LogoutCommand = new Command(async () => await LogoutAsync());
		NavigateToProfileCommand = new Command(async () => await NavigateToProfileAsync());
		_ = GetUserInfoAsync();

	}

	private async Task LogoutAsync()
	{
		try
		{
			SecureStorage.Remove("auth_token");
			SecureStorage.Remove("user_id");
			Console.WriteLine($"Logout didnt fail?");


			await Shell.Current.GoToAsync(nameof(Login));
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Logout failed: {ex.Message}");
		}
	}

	public async Task GetUserInfoAsync()
	{
		var userInfo = await _userService.GetuserInfoAsync();
		if (userInfo != null)
		{
			FullName = $"{userInfo.FirstName} {userInfo.LastName}";
			Email = $"{userInfo.Email}";
		}
	}

	private async Task NavigateToProfileAsync()
	{
		await Shell.Current.GoToAsync(nameof(Profile));
	}
}