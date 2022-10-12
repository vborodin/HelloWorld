using AuthenticationService.Controllers;
using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services;
using AuthenticationService.Services.Model;
using AuthenticationService.TokenGenerator;

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
        this.data = new List<UserModel>();
        this.tokenGeneratorMock = CreateTokenGeneratorMock();
        this.userServiceMock = CreateUserServiceMock();
        this.controller = new AccountController(this.userServiceMock.Object, this.tokenGeneratorMock.Object);
    }

    [Test]
    public void LoginReturnsOKAndTokenWhenSuccessfullyLoggedIn()
    {
        var result = this.controller.Login(
            userLogin: new UserLoginDto(Username: "ExistingUser", Password: "ValidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: 10) as OkObjectResult;
        var token = result!.Value as string;

        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        Assert.AreEqual("ExistingUser:TestAudience:10", token);
    }

    [Test]
    public void LoginReturnsUnauthorizedWithIncorrectLoginOrPassword()
    {
        var result = this.controller.Login(
            userLogin: new UserLoginDto(Username: "InvalidUser", Password: "InvalidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: 10) as UnauthorizedResult;

        Assert.AreEqual(StatusCodes.Status401Unauthorized, result!.StatusCode);
    }

    [Test]
    public void LoginReturnsBadRequestWithNonPositiveExpirationPeriod()
    {
        var result1 = this.controller.Login(
            userLogin: new UserLoginDto(Username: "ExistingUser", Password: "ValidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: 0) as BadRequestObjectResult;

        Assert.AreEqual(StatusCodes.Status400BadRequest, result1!.StatusCode);

        var result2 = this.controller.Login(
            userLogin: new UserLoginDto(Username: "ExistingUser", Password: "ValidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: -1) as BadRequestObjectResult;

        Assert.AreEqual(StatusCodes.Status400BadRequest, result2!.StatusCode);
    }

    [Test]
    public void LoginReturnsBadRequestWithEmptyAudience()
    {
        var result = this.controller.Login(
            userLogin: new UserLoginDto(Username: "ExistingUser", Password: "ValidPassword"),
            audience: "",
            expirationPeriodMinutes: 1) as BadRequestObjectResult;

        Assert.AreEqual(StatusCodes.Status400BadRequest, result!.StatusCode);
    }

    [Test]
    public void RegisterReturnsOk()
    {
        var result = this.controller.Register(
            userRegistrationDto: new UserRegistrationDto(
                Username: "NewUsername",
                Password: "NewPassword",
                Email: "NewEmail",
                GivenName: "NewGivenName",
                Surname: "NewSurname")) as OkResult;

        Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        var user = this.data.Single();
        Assert.AreEqual("NewUsername", user.Username);
        Assert.AreEqual("NewEmail", user.Email);
        Assert.AreEqual("User", user.Role);
        Assert.AreEqual("NewSurname", user.Surname);
        Assert.AreEqual("NewGivenName", user.GivenName);
    }

    private Mock<IUserService> CreateUserServiceMock()
    {
        var mock = new Mock<IUserService>();
        mock.Setup(m => m.GetUserAsync("ExistingUser", "ValidPassword"))
            .Returns(() => { return new UserModel(
                Username: "ExistingUser",
                Email: "ValidEmail",
                Role: "ValidRole",
                Surname: "ValidSurname",
                GivenName: "ValidGivenName"); });
        mock.Setup(m => m.GetUserAsync("InvalidUser", "InvalidPassword"))
            .Returns(() => null);
        mock.Setup(m => m.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string, string, string, string>((username, password, email, givenName, surname) => 
                this.data.Add(new UserModel(Username: username, Email: email, Role: "User", Surname: surname, GivenName: givenName)));
        return mock;
    }

    private Mock<ITokenGenerator<UserModel>> CreateTokenGeneratorMock()
    {
        var mock = new Mock<ITokenGenerator<UserModel>>();
        mock.Setup(m => m.Generate(It.IsAny<UserModel>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns((UserModel m, string audience, int expirationPeriod) => $"{m.Username}:{audience}:{expirationPeriod}");
        return mock;
    }
}
