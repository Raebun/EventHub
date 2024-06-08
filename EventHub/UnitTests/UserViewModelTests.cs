namespace UnitTests;

public class UserViewModelTests
{
    [Fact]
    public async Task GetUserInfoAsync_ShouldPopulateUpdateUser()
    {
        // Arrange
        var userService = Substitute.For<IUserService>();
        var messagingService = Substitute.For<MessagingService>();
        var userInfo = new UserInfo
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            ProfilePictureUrl = "http://example.com/john.jpg"
        };
        userService.GetuserInfoAsync().Returns(Task.FromResult(userInfo));
        var viewModel = new UserViewModel(userService, messagingService);


        // Act
        await viewModel.GetUserInfoAsync();

        // Assert
        viewModel.UpdateUser.Firstname.Should().Be("John");
        viewModel.UpdateUser.Lastname.Should().Be("Doe");
        viewModel.UpdateUser.Email.Should().Be("john.doe@example.com");
        viewModel.ProfilePictureUrl.Should().Be("http://example.com/john.jpg");
    }
}