using AuthenticationService.Controllers.Dtos;
using AuthenticationService.Services.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

namespace AuthenticationService.Tests.Controllers.AccountControllerMethods;

public class SetRoleAsync: AccountControllerTest
{
    [Test]
    public async Task SetsRole()
    {
        var result = await this.controller.SetRoleAsync(
            setRoleDto: new SetRoleDto(
                Username: "ExistingUser",
                Role: "NewRole"));

        Assert.True(result is OkResult);
        this.userServiceMock.Verify(s => s.AddRoleAsync("ExistingUser", "NewRole"), Times.Once);
    }

    [Test]
    public async Task RequiresExistingUser()
    {
        this.userServiceMock
            .Setup(m => m.AddRoleAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws<RoleAssignmentException>();

        var result = await this.controller.SetRoleAsync(new SetRoleDto(
            Username: "InvalidUser",
            Role: "NewRole"));

        Assert.True(result is BadRequestObjectResult);
    }
}
