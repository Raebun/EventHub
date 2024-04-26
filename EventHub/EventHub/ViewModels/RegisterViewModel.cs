using CommunityToolkit.Mvvm.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace EventHub.ViewModels
{
	public class RegisterViewModel : ObservableObject
	{
		private string _firstName;
		private string _lastName;
		private string _email;
		private string _password;
		private string _errorMessage;

		public ICommand RegisterCommand { get; }
		public event EventHandler RegisterSuccess;
		private readonly HttpClient _httpClient;

		public RegisterViewModel(HttpClient httpClient)
		{
			_httpClient = httpClient;
			RegisterCommand = new Command(async () => await RegisterAsync());
		}

		public string Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}

		public string Password
		{
			get => _password;
			set => SetProperty(ref _password, value);
		}

		public string FirstName
		{
			get => _firstName;
			set => SetProperty(ref _firstName, value);
		}

		public string LastName
		{
			get => _lastName;
			set => SetProperty(ref _lastName, value);
		}

		public string ErrorMessage
		{
			get => _errorMessage;
			set => SetProperty(ref _errorMessage, value);
		}

		public async Task RegisterAsync()
		{
			var registerModel = new
			{
				FirstName,
				LastName,
				Email,
				Password
			};
			var json = JsonSerializer.Serialize(registerModel);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
			var response = await _httpClient.PostAsync(uri + "Auth/registerUser", content);

			if (response.IsSuccessStatusCode)
			{
				RegisterSuccess?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				HandleRegistrationFailure();
			}
		}

		private void HandleRegistrationFailure()
		{
			ErrorMessage = "Registration failed.";
		}
	}
}
