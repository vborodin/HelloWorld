using AuthenticationService.Controllers;
using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services;
using AuthenticationService.Services.Exceptions;
using AuthenticationService.Services.Model;
using AuthenticationService.Services.TokenGenerator;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers;

public class AccountControllerTest
{
    private Mock<ITokenGenerator<UserModel>> tokenGeneratorMock = null!;
    private Mock<IUserService> userServiceMock = null!;
    private List<UserModel> data = null!;
    private AccountController controller = null!;

    [SetUp]
    public void Setup()
    {
        this.data = CreateTestData();
        this.tokenGeneratorMock = CreateTokenGeneratorMock();
        this.userServiceMock = CreateUserServiceMock();
        this.controller = new AccountController(this.userServiceMock.Object, this.tokenGeneratorMock.Object);
    }

    [Test]
    public async Task LoginReturnsOKAndTokenWhenSuccessfullyLoggedIn()
    {
        var result = await this.controller.LoginAsync(
            userLogin: new UserLoginDto(Username: "ExistingUser", Password: "ValidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: 10) as OkObjectResult;
        var token = result!.Value as string;

        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        Assert.AreEqual("ExistingUser:TestAudience:10:ValidRole", token);
    }

    [Test]
    public async Task LoginReturnsUnauthorizedWithIncorrectLoginOrPassword()
    {
        var result = await this.controller.LoginAsync(
            userLogin: new UserLoginDto(Username: "InvalidUser", Password: "InvalidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: 10);
        var unauthorized = result as UnauthorizedObjectResult;

        Assert.AreEqual(StatusCodes.Status401Unauthorized, unauthorized!.StatusCode);
    }

    [Test]
    public async Task LoginReturnsBadRequestWithNonPositiveExpirationPeriod()
    {
        var result1 = await this.controller.LoginAsync(
            userLogin: new UserLoginDto(Username: "ExistingUser", Password: "ValidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: 0) as BadRequestObjectResult;

        Assert.AreEqual(StatusCodes.Status400BadRequest, result1!.StatusCode);

        var result2 = await this.controller.LoginAsync(
            userLogin: new UserLoginDto(Username: "ExistingUser", Password: "ValidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: -1) as BadRequestObjectResult;

        Assert.AreEqual(StatusCodes.Status400BadRequest, result2!.StatusCode);
    }

    [Test]
    public async Task RegisterReturnsOk()
    {
        var result = await this.controller.Register(
            userRegistrationDto: new UserRegistrationDto(
                Username: "NewUsername",
                Password: "NewPassword",
                Email: "NewEmail",
                GivenName: "NewGivenName",
                Surname: "NewSurname")) as OkResult;

        Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        var user = this.data.Single(x => x.Username == "NewUsername");
        Assert.AreEqual("NewUsername", user.Username);
        Assert.AreEqual("NewEmail", user.Email);
        Assert.AreEqual("User", user.Role);
        Assert.AreEqual("NewSurname", user.Surname);
        Assert.AreEqual("NewGivenName", user.GivenName);
    }

    [Test]
    public async Task SetRoleReturnsOk()
    {
        var setRoleResult = await this.controller.SetRole(new SetRoleDto(
            Username: "ExistingUser",
            Role: "NewRole")) as OkResult;
        Assert.AreEqual(StatusCodes.Status200OK, setRoleResult!.StatusCode);
        var loginResult = await this.controller.LoginAsync(
            userLogin: new UserLoginDto(Username: "ExistingUser", Password: "ValidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: 10) as OkObjectResult;
        var token = loginResult!.Value;

        Assert.AreEqual("ExistingUser:TestAudience:10:NewRole", token);
    }

    [Test]
    public async Task SetRoleReturnsBadRequestForInvalidUsername()
    {
        var setRoleResult = await this.controller.SetRole(new SetRoleDto(
            Username: "InvalidUser",
            Role: "NewRole")) as BadRequestObjectResult;

        Assert.AreEqual(StatusCodes.Status400BadRequest, setRoleResult!.StatusCode);
    }

    private Mock<IUserService> CreateUserServiceMock()
    {
        var mock = new Mock<IUserService>();
        mock.Setup(m => m.GetUserAsync("ExistingUser", "ValidPassword"))
            .Returns(() => Task.FromResult<UserModel?>(this.data.Single(x => x.Username == "ExistingUser")));
        mock.Setup(m => m.GetUserAsync("InvalidUser", "InvalidPassword"))
            .Returns(() => Task.FromResult<UserModel?>(null!));
        mock.Setup(m => m.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string, string, string, string>((username, password, email, givenName, surname) => 
                this.data.Add(new UserModel(Username: username, Email: email, Role: "User", Surname: surname, GivenName: givenName)));
        mock.Setup(m => m.SetRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((username, role) =>
            {
                var index = this.data.FindIndex(x => x.Username == username);
                if (index < 0)
                {
                    throw new RoleAssignmentException();
                }
                this.data[index] = this.data[index] with { Role = role };
            });
        return mock;
    }

    private Mock<ITokenGenerator<UserModel>> CreateTokenGeneratorMock()
    {
        var mock = new Mock<ITokenGenerator<UserModel>>();
        mock.Setup(m => m.Generate(It.IsAny<UserModel>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns((UserModel m, string audience, int expirationPeriod) => $"{m.Username}:{audience}:{expirationPeriod}:{m.Role}");
        return mock;
    }

    private List<UserModel> CreateTestData()
    {
        return new()
        {
            new UserModel(
                Username: "ExistingUser",
                Email: "ValidEmail",
                Role: "ValidRole",
                Surname: "ValidSurname",
                GivenName: "ValidGivenName")
        };
    }
}
