using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services.Exceptions;
using AuthenticationService.Services.Model;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.AccountControllerMethods;

public class RegisterAsync: AccountControllerTest
{
    [Test]
    public async Task CreatesNewUser()
    {
        var data = new List<UserModel>();
        this.userServiceMock
            .Setup(m => m.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Callback<string, string>((username, _) =>
            {
                data.Add(new UserModel(username, new List<string>()));
            });

        var result = await this.controller.RegisterAsync(
            userPasswordDto: new UserPasswordDto(
                Username: "NewUsername",
                Password: "NewPassword"));
        var user = data.Single(x => x.Username == "NewUsername");

        Assert.True(result is OkResult);
        Assert.AreEqual("NewUsername", user.Username);
        Assert.NotNull(user.Roles);
    }

    [Test]
    public async Task RequiresUniqueUsername()
    {
        this.userServiceMock
            .Setup(m => m.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RegistrationException());

        var result = await this.controller.RegisterAsync(
            userPasswordDto: new UserPasswordDto(
                Username: "ExistingUser",
                Password: "NewPassword"));

        Assert.True(result is BadRequestObjectResult);
    }
}
