using System.Net;
using System.Text;
using System.Text.Json;
using UnitTests.Mocks;

namespace UnitTests;

public class RegisterViewModelTests
{
    private HttpClient CreateHttpClient(Func<HttpRequestMessage, Task<HttpResponseMessage>> sendAsync)
    {
        var handler = new HttpMessageHandlerMock(sendAsync);
        return new HttpClient(handler);
    }

    [Fact]
    public async Task RegisterAsync_ShouldInvokeRegisterSuccessOnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        var httpClient = CreateHttpClient(request => Task.FromResult(responseMessage));
        var viewModel = new RegisterViewModel(httpClient);
        bool registerSuccessInvoked = false;
        viewModel.RegisterSuccess += (sender, args) => registerSuccessInvoked = true;

        // Act
        viewModel.FirstName = "John";
        viewModel.LastName = "Doe";
        viewModel.Email = "john.doe@example.com";
        viewModel.Password = "Password123";
        await viewModel.RegisterAsync();

        // Assert
        registerSuccessInvoked.Should().BeTrue();
        viewModel.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public async Task RegisterAsync_ShouldSetErrorMessageOnFailure()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
        var httpClient = CreateHttpClient(request => Task.FromResult(responseMessage));
        var viewModel = new RegisterViewModel(httpClient);
        bool registerSuccessInvoked = false;
        viewModel.RegisterSuccess += (sender, args) => registerSuccessInvoked = true;

        // Act
        viewModel.FirstName = "John";
        viewModel.LastName = "Doe";
        viewModel.Email = "john.doe@example.com";
        viewModel.Password = "Password123";
        await viewModel.RegisterAsync();

        // Assert
        registerSuccessInvoked.Should().BeFalse();
        viewModel.ErrorMessage.Should().Be("Registration failed.");
    }
}
