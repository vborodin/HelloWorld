using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services.Model;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.AccountControllerMethods;

public class LoginAsync: AccountControllerTest
{
    [Test]
    public async Task GeneratesJWTToken()
    {
        this.tokenGeneratorMock
            .Setup(m => m.Generate(
                It.IsAny<UserModel>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
            .Returns<UserModel, string, int>((m, audience, expirationPeriod) =>
            {
                return $"{m.Username}:{audience}:{expirationPeriod}";
            });
        this.userServiceMock
            .Setup(m => m.GetUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns<string, string>((username, _) =>
                Task.FromResult<UserModel?>(
                    result: new UserModel(
                        Username: username,
                        Roles: new List<string>())));

        var result = await this.controller.LoginAsync(
            usernamePasswordDto: new UsernamePasswordDto(
                Username: "ExistingUser",
                Password: "ValidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: 10);

        Assert.True(result is OkObjectResult);
        Assert.AreEqual("ExistingUser:TestAudience:10", (result as OkObjectResult)!.Value);
    }

    [Test]
    public async Task RequiresValidUsernameAndPassword()
    {
        this.userServiceMock
            .Setup(m => m.GetUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(Task.FromResult<UserModel?>(null));

        var result = await this.controller.LoginAsync(
            usernamePasswordDto: new UsernamePasswordDto(
                Username: "InvalidUser",
                Password: "InvalidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: 10);

        Assert.True(result is UnauthorizedObjectResult);
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(int.MinValue)]
    public async Task RequiresPositiveExpirationPeriod(int expirationPeriod)
    {
        var result = await this.controller.LoginAsync(
            usernamePasswordDto: new UsernamePasswordDto(
                Username: "ExistingUser",
                Password: "ValidPassword"),
            audience: "TestAudience",
            expirationPeriodMinutes: expirationPeriod);

        Assert.True(result is BadRequestObjectResult);
    }
}
