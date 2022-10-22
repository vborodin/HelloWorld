using AuthenticationService.Controllers;
using AuthenticationService.Services;
using AuthenticationService.Services.Model;
using AuthenticationService.Services.TokenGenerator;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.AccountControllerMethods;

[Category("Controllers")]
public abstract class AccountControllerTest
{
    protected AccountController controller = null!;
    protected Mock<IUserService> userServiceMock = null!;
    protected Mock<ITokenGenerator<UserModel>> tokenGeneratorMock = null!;

    [SetUp]
    public void Setup()
    {
        this.userServiceMock = new Mock<IUserService>();
        this.tokenGeneratorMock = new Mock<ITokenGenerator<UserModel>>();
        this.controller = new AccountController(this.userServiceMock.Object, this.tokenGeneratorMock.Object);
    }
}