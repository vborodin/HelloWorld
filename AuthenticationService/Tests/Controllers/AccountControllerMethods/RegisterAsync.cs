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
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Callback<string, string, string, string, string>((username, _, email, givenname, surname) =>
            {
                data.Add(new UserModel(username, email, "", surname, givenname));
            });

        var result = await this.controller.RegisterAsync(
            userRegistrationDto: new UserRegistrationDto(
                Username: "NewUsername",
                Password: "NewPassword",
                Email: "NewEmail",
                GivenName: "NewGivenName",
                Surname: "NewSurname"));
        var user = data.Single(x => x.Username == "NewUsername");

        Assert.True(result is OkResult);
        Assert.AreEqual("NewUsername", user.Username);
        Assert.AreEqual("NewEmail", user.Email);
        Assert.AreEqual("NewSurname", user.Surname);
        Assert.AreEqual("NewGivenName", user.GivenName);
    }

    [Test]
    public async Task RequiresUniqueUsername()
    {
        this.userServiceMock
            .Setup(m => m.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(new RegistrationException());

        var result = await this.controller.RegisterAsync(
            userRegistrationDto: new UserRegistrationDto(
                Username: "ExistingUser",
                Password: "NewPassword",
                Email: "NewEmail",
                GivenName: "NewGivenName",
                Surname: "NewSurname"));

        Assert.True(result is BadRequestObjectResult);
    }
}
